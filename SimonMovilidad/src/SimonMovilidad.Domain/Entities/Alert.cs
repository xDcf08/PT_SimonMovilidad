using SimonMovilidad.Domain.Enums;

namespace SimonMovilidad.Domain.Entities
{
    public sealed class Alert
    {
        public Guid Id { get; init; }
        public AlertTypeEnum AlertType { get; init; }
        public string? Message { get; init; }
        public DateTime TimeStamp { get; init; }
        public bool IsResolved { get; set; }

        public Guid VehicleId { get; init; }
        public Vehicle? Vehicle { get; init; }
    }
}
