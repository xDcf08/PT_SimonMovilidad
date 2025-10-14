using Dapper;
using Microsoft.AspNetCore.Http;
using SimonMovilidad.Application.Abstractions.Data;
using SimonMovilidad.Application.Abstractions.Messaging;
using SimonMovilidad.Domain.Abstractions;
using SimonMovilidad.Domain.Enums;
using System.Security.Claims;

namespace SimonMovilidad.Application.Features.Vehicles.GetLiveLocation
{
    internal sealed class GetLiveLocationQueryHandler : IQueryHandler<GetLiveLocationQuery, IEnumerable<VehicleLocationResponse>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetLiveLocationQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IHttpContextAccessor httpContextAccessor)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<IEnumerable<VehicleLocationResponse>>> Handle(GetLiveLocationQuery request, CancellationToken cancellationToken)
        {
            var userRole = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role);
            var isAdmin = userRole?.Value == RoleEnum.Admin.ToString();

            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                WITH LatestLocations AS (
                    SELECT
                        sr.vehicle_id AS VehicleId,
                        sr.latitude AS Latitude,
                        sr.longitude AS Longitude,
                        sr.fuel_level AS FuelLevel,
                        ROW_NUMBER() OVER(PARTITION BY sr.vehicle_id ORDER BY sr.timestamp DESC) as rn
                    FROM
                     sensor_readings sr
                )

                SELECT
                    v.id AS VehicleId,
                    v.device_id AS DeviceId,
                    lr.latitude AS Latitude,
                    lr.longitude AS Longitude,
                    lr.FuelLevel AS FuelLevel
                FROM Vehicles v
                JOIN LatestLocations lr ON v.id = lr.VehicleId
                WHERE lr.rn = 1;
                """;

            var liveLocation = await connection.QueryAsync<VehicleLocationResponse>(sql);

            liveLocation = liveLocation.Select(v => new VehicleLocationResponse
            {
                VehicleId = v.VehicleId,
                DeviceId = isAdmin ? v.DeviceId : MaskDeviceId(v.DeviceId!),
                Latitude = v.Latitude,
                Longitude = v.Longitude,
                FuelLevel = v.FuelLevel
            });

            return Result.Success(liveLocation);
        }

        private static string MaskDeviceId(string deviceId)
        {
            if (string.IsNullOrEmpty(deviceId) || deviceId.Length < 8)
            {
                return deviceId;
            }

            return $"{deviceId.Substring(0,4)}****{deviceId.Substring(deviceId.Length - 4)}";
        }
    }
}
