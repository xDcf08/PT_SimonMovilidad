using SimonMovilidad.Application.Abstractions.Messaging;
using SimonMovilidad.Application.Abstractions.Notifier;
using SimonMovilidad.Application.Features.Vehicles.GetLiveLocation;
using SimonMovilidad.Domain.Abstractions;
using SimonMovilidad.Domain.Abstractions.Errors;
using SimonMovilidad.Domain.Entities;
using SimonMovilidad.Domain.Enums;
using SimonMovilidad.Domain.Repository;

namespace SimonMovilidad.Application.Features.Telemetry.CreateTelemetry
{
    internal sealed class CreateTelemetryCommandHandler : ICommandHandler<CreateTelemetryCommand>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ISensorReadingRepository _sensorReadingRepository;
        private readonly IAlertRepository _alertRepository;
        private readonly ITelemetryNotifier _telemetryNotifier;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTelemetryCommandHandler(
            IVehicleRepository vehicleRepository,
            ISensorReadingRepository sensorReadingRepository,
            IAlertRepository alertRepository,
            ITelemetryNotifier telemetryNotifier,
            IUnitOfWork unitOfWork)
        {
            _vehicleRepository = vehicleRepository;
            _sensorReadingRepository = sensorReadingRepository;
            _alertRepository = alertRepository;
            _telemetryNotifier = telemetryNotifier;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(CreateTelemetryCommand request, CancellationToken cancellationToken)
        {
            //1. Buscar el vehículo por DeviceId
            var vehicle = await _vehicleRepository.GetByDeviceIdAsync(request.DeviceId, cancellationToken);

            if (vehicle is null)
            {
                return Result.Failure(VehicleError.NotFound);
            }

            //2. Crear la entidad se SensorReading
            var sensorReading = new SensorReading
            {
                VehicleId = vehicle!.Id,
                TimeStamp = request.Timestamp,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                FuelLevel = request.FuelLevel,
                Temperature = request.Temperature
            };
            await _sensorReadingRepository.AddAsync(sensorReading);

            //3. Verificar si el nivel de combustible es menor al umbral y crear una alerta si es necesario
            if (vehicle.AvgConsumption > 0)
            {
                var autonomyInHours = request.FuelLevel / vehicle.AvgConsumption;
                if (autonomyInHours < 1)
                {
                    var alert = new Alert
                    {
                        VehicleId = vehicle.Id,
                        AlertType = AlertTypeEnum.PredictiveLowFuel,
                        Message = "El nivel de combustible es crítico.",
                        TimeStamp = DateTime.UtcNow,
                    };
                    await _alertRepository.AddAsync(alert);
                }
            }

            //4. Guardar los cambios en la base de datos
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            //5. Avisar al usuario si el nivel de combustible es crítico a través de SignalR.
            var locationUpdate = new VehicleLocationResponse
            {
                VehicleId = vehicle.Id,
                DeviceId = vehicle.DeviceId,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                FuelLevel = request.FuelLevel
            };

            await _telemetryNotifier.NotifyNewReadingAsync(locationUpdate);

            //6. Retornar el resultado
            return Result.Success();
        }
    }
}
