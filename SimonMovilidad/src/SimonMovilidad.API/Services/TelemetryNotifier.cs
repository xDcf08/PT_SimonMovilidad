using Microsoft.AspNetCore.SignalR;
using SimonMovilidad.API.Hubs;
using SimonMovilidad.Application.Abstractions.Notifier;
using SimonMovilidad.Application.Features.Vehicles.GetLiveLocation;

namespace SimonMovilidad.API.Services
{
    internal sealed class TelemetryNotifier : ITelemetryNotifier
    {
        private readonly IHubContext<TelemetryHub> _hubContext;
        private readonly ILogger<TelemetryNotifier> _logger;

        public TelemetryNotifier(IHubContext<TelemetryHub> hubContext, ILogger<TelemetryNotifier> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task NotifyNewReadingAsync(VehicleLocationResponse locationUpdate)
        {
            _logger.LogInformation(
                "Enviando actualización de telemetría para el vehículo {VehicleId} a todos los clientes.",
                locationUpdate.VehicleId
            );

            await _hubContext.Clients.All.SendAsync("ReceiveLocationUpdate", locationUpdate);
        }
    }
}
