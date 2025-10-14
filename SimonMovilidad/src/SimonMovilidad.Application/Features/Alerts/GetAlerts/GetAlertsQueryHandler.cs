using Dapper;
using SimonMovilidad.Application.Abstractions.Data;
using SimonMovilidad.Application.Abstractions.Messaging;
using SimonMovilidad.Domain.Abstractions;

namespace SimonMovilidad.Application.Features.Alerts.GetAlerts
{
    internal sealed class GetAlertsQueryHandler : IQueryHandler<GetAlertsQuery,IEnumerable<AlertsResponse>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetAlertsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<IEnumerable<AlertsResponse>>> Handle(GetAlertsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT
                    a.id AS "AlertId",
                    a.vehicle_id AS "VehicleId",
                    v.license_plate AS "LicensePlate",
                    a.alert_type AS "AlertType",
                    a.message AS "Message",
                    a."timestamp" AS "Timestamp"
                FROM Alerts a
                INNER JOIN Vehicles v ON a.vehicle_id = v.id
                WHERE a.is_resolved = false
                ORDER BY a."timestamp" DESC;
                """;

            var alerts = await connection.QueryAsync<AlertsResponse>(sql);

            return Result.Success(alerts);
        }
    }
}
