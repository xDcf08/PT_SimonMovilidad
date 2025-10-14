namespace SimonMovilidad.Domain.Entities
{
    public sealed class Vehicle
    {
        public Guid Id { get; init; }
        public required string DeviceId { get; init; }
        public string? LicensePlate {  get; init; }
        public decimal AvgConsumption { get; init; }
        public DateTime CreateAt { get; init; }

        public ICollection<SensorReading> SensorReadings { get; init; } = new List<SensorReading>();
        public ICollection<Alert> Alerts { get; init; } = new List<Alert>();
    }
}
