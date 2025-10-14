using SimonMovilidad.Application.Abstractions.Messaging;
using SimonMovilidad.Domain.Abstractions;

namespace SimonMovilidad.Application.Features.Vehicles.GetLiveLocation
{
    public record GetLiveLocationQuery() : IQuery<IEnumerable<VehicleLocationResponse>>;
}
