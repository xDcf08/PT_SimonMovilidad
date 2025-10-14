namespace SimonMovilidad.Domain.Entities
{
    public sealed class SensorReading
    {
        public long Id { get; init; }
        public DateTime TimeStamp { get; init; }
        public decimal Latitude { get; init; }
        public decimal Longitude { get; init; }
        public decimal FuelLevel { get; init; }
        public decimal? Temperature { get; init; }

        public Guid VehicleId { get; init; }
        public Vehicle? Vehicle { get; init; }
    }
}
