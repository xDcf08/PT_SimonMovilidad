namespace SimonMovilidad.Application.Features.Alerts.GetAlerts
{
    public class AlertsResponse
    {
        public Guid AlertId { get; init; }
        public Guid VehicleId { get; init; }
        public string? AlertType { get; init; }
        public string? LicensePlate { get; init; }
        public string? Message { get; init; }
        public DateTime Timestamp { get; init; }
    }
}
