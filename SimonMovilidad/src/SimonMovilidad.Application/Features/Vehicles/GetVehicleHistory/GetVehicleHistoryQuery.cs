using SimonMovilidad.Application.Abstractions.Messaging;

namespace SimonMovilidad.Application.Features.Vehicles.GetVehicleHistory
{
    public record GetVehicleHistoryQuery(Guid VehicleId, DateTime StartTime, DateTime EndTime) : IQuery<IEnumerable<HistoricalDataPointResponse>>;
}
