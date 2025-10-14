using SimonMovilidad.Application.Features.Vehicles.GetLiveLocation;

namespace SimonMovilidad.Application.Abstractions.Notifier
{
    public interface ITelemetryNotifier
    {
        Task NotifyNewReadingAsync(VehicleLocationResponse locationUpdate);
    }
}
