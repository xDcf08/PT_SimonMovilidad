using NSubstitute;
using SimonMovilidad.Application.Abstractions.Notifier;
using SimonMovilidad.Application.Features.Telemetry.CreateTelemetry;
using SimonMovilidad.Domain.Abstractions;
using SimonMovilidad.Domain.Entities;
using SimonMovilidad.Domain.Repository;
using Xunit;

namespace SimonMovilidad.Application.UnitTest.Telemetry
{
    public class CreateTelemetryTest
    {
        private readonly CreateTelemetryCommandHandler _handlerMock;

        private readonly IVehicleRepository _vehicleRepositoryMock;
        private readonly ISensorReadingRepository _sensorReadingRepositoryMock;
        private readonly IAlertRepository _alertRepositoryMock;
        private readonly ITelemetryNotifier _telemetryNotifierMock;
        private readonly IUnitOfWork _unitOfWorkMock;

        //private readonly CreateTelemetryCommand Command = new(
        //    "TEST-DEV-001", DateTime.UtcNow, 0, 0, 11.0m, null
        //    );

        public CreateTelemetryTest()
        {
            _vehicleRepositoryMock = Substitute.For<IVehicleRepository>();
            _sensorReadingRepositoryMock = Substitute.For<ISensorReadingRepository>();
            _alertRepositoryMock = Substitute.For<IAlertRepository>();
            _telemetryNotifierMock = Substitute.For<ITelemetryNotifier>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            _handlerMock = new CreateTelemetryCommandHandler(
                _vehicleRepositoryMock,
                _sensorReadingRepositoryMock,
                _alertRepositoryMock,
                _telemetryNotifierMock,
                _unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_CreateAlert_WhenFuelAutonomyIsLessThanOneHour()
        {
            //Arrenge
            var vehicle = new Vehicle { 
                Id = Guid.NewGuid(),
                DeviceId = "TEST-DEV-001",
                AvgConsumption = 10.0m
            };

            _vehicleRepositoryMock
                .GetByDeviceIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(vehicle);

            var commandWithLowFuel = new CreateTelemetryCommand(vehicle.DeviceId, DateTime.UtcNow, 0, 0, 9.0m, null);

            //ACT
            await _handlerMock.Handle(commandWithLowFuel, CancellationToken.None);

            //ASSERT
            await _alertRepositoryMock
                .Received(1)
                .AddAsync(Arg.Is<Alert>(a => a.VehicleId == vehicle.Id));
        }

        [Fact]
        public async Task Handle_Should_NotCreateAlert_WhenFuelAutonomyIsMoreThanOneHout()
        {
            //Arrenge
            var vehicle = new Vehicle
            {
                Id = Guid.NewGuid(),
                DeviceId = "TEST-DEV-002",
                AvgConsumption = 10.0m
            };

            _vehicleRepositoryMock
                .GetByDeviceIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(vehicle);

            var commandWithHighFuel = new CreateTelemetryCommand(vehicle.DeviceId, DateTime.UtcNow, 0, 0, 11.0m, null);

            //ACT
            await _handlerMock.Handle(commandWithHighFuel, CancellationToken.None);

            //ASSERT
            await _alertRepositoryMock
                .DidNotReceive()
                .AddAsync(Arg.Any<Alert>());
        }
    }
}
