using SimonMovilidad.Application.Abstractions.Messaging;

namespace SimonMovilidad.Application.Features.Alerts.GetAlerts
{
    public record GetAlertsQuery : IQuery<IEnumerable<AlertsResponse>>;
}
