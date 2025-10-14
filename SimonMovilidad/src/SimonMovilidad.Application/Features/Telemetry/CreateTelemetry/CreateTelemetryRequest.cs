namespace SimonMovilidad.Application.Features.Telemetry.CreateTelemetry
{
    public record CreateTelemetryRequest(
        string DeviceId,
        DateTime Timestamp,
        decimal Latitude,
        decimal Longitude,
        decimal FuelLevel,
        decimal? Temperature);
}
