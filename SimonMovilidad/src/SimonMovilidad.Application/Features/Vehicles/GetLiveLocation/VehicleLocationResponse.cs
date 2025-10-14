namespace SimonMovilidad.Application.Features.Vehicles.GetLiveLocation
{
    public class VehicleLocationResponse
    {
        public Guid VehicleId { get; init; }
        public string? DeviceId { get; init; }
        public decimal Latitude { get; init; }
        public decimal Longitude { get; init; }
        public decimal FuelLevel { get; init; }
    }
}
