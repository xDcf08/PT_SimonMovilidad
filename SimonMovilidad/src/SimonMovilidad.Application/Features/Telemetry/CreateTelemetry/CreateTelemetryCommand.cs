using SimonMovilidad.Application.Abstractions.Messaging;
using SimonMovilidad.Domain.Abstractions;

namespace SimonMovilidad.Application.Features.Telemetry.CreateTelemetry
{
    public record CreateTelemetryCommand(
        string DeviceId,
        DateTime Timestamp,
        decimal Latitude,
        decimal Longitude,
        decimal FuelLevel,
        decimal? Temperature) : ICommand;
}
