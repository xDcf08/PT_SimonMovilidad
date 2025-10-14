using SimonMovilidad.Application.Abstractions.Messaging;

namespace SimonMovilidad.Application.Features.Vehicles.CreateVehicle
{
    public record CreateVehicleCommand(string DeviceId, string LicensePlate, decimal AvgConsumption) : ICommand<Guid>;
}
