using SimonMovilidad.Application.Abstractions.Messaging;
using SimonMovilidad.Domain.Abstractions;
using SimonMovilidad.Domain.Abstractions.Errors;
using SimonMovilidad.Domain.Entities;
using SimonMovilidad.Domain.Repository;

namespace SimonMovilidad.Application.Features.Vehicles.CreateVehicle
{
    internal sealed class CreateVehicleCommandHandler : ICommandHandler<CreateVehicleCommand, Guid>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateVehicleCommandHandler(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork)
        {
            _vehicleRepository = vehicleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
        {
            var existingVehicleByLicensePlate = await _vehicleRepository.GetByLicensePlateAsync(request.LicensePlate, cancellationToken);

            if (existingVehicleByLicensePlate is not null)
            {
                return Result.Failure<Guid>(VehicleError.AlreadyExistsByLicensePlate);
            }

            var existingVehicleByDeviceId = await _vehicleRepository.GetByDeviceIdAsync(request.DeviceId, cancellationToken);

            if (existingVehicleByDeviceId is not null)
            {
                return Result.Failure<Guid>(VehicleError.DeviceIdAlreadyInUse);
            }

            var newVehicle = new Vehicle
            {
                DeviceId = request.DeviceId,
                LicensePlate = request.LicensePlate,
                AvgConsumption = request.AvgConsumption,
                CreateAt = DateTime.UtcNow,
            };

            await _vehicleRepository.AddAsync(newVehicle);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return newVehicle.Id;
        }
    }
}
