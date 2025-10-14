using FluentAssertions;
using NSubstitute;
using SimonMovilidad.Application.Features.Vehicles.CreateVehicle;
using SimonMovilidad.Domain.Abstractions;
using SimonMovilidad.Domain.Abstractions.Errors;
using SimonMovilidad.Domain.Entities;
using SimonMovilidad.Domain.Repository;
using Xunit;

namespace SimonMovilidad.Application.UnitTest.Vehicles
{
    public class CreateVehicleCommandHandlerTest
    {
        private readonly CreateVehicleCommandHandler _handler;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateVehicleCommandHandlerTest()
        {
            _vehicleRepository = Substitute.For<IVehicleRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _handler = new CreateVehicleCommandHandler(
                _vehicleRepository,
                _unitOfWork
                );
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenDeviceIdAlreadyExists()
        {
            //Arrange
            var command = new CreateVehicleCommand("DEV-ALREADY-EXISTS", "TTT111", 10.0m);

            _vehicleRepository.GetByDeviceIdAsync(command.DeviceId,Arg.Any<CancellationToken>())
                .Returns(new Vehicle { DeviceId = command.DeviceId });

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.Error.Should().Be(VehicleError.DeviceIdAlreadyInUse);

            await _vehicleRepository
                .DidNotReceive()
                .AddAsync(Arg.Any<Vehicle>());

            await _unitOfWork
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenLicensePlateAlreadyExists()
        {
            //Arrange
            var command = new CreateVehicleCommand("DEV-1234-1234", "TTT111", 10.0m);

            _vehicleRepository.GetByLicensePlateAsync(command.LicensePlate, Arg.Any<CancellationToken>())
                .Returns(new Vehicle { DeviceId = command.DeviceId, LicensePlate = command.LicensePlate});

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.Error.Should().Be(VehicleError.AlreadyExistsByLicensePlate);
            await _vehicleRepository
                .DidNotReceive()
                .AddAsync(Arg.Any<Vehicle>());

            await _unitOfWork
                .DidNotReceive()
                .SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
