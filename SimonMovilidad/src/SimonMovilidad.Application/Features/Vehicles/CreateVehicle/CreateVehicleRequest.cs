namespace SimonMovilidad.Application.Features.Vehicles.CreateVehicle
{
    public record CreateVehicleRequest(string DeviceId, string LicensePlate, decimal AvgConsumption);
}
