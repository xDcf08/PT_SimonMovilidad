using Dapper;
using SimonMovilidad.Application.Abstractions.Data;
using SimonMovilidad.Application.Abstractions.Messaging;
using SimonMovilidad.Domain.Abstractions;
using SimonMovilidad.Domain.Abstractions.Errors;
using SimonMovilidad.Domain.Entities;

namespace SimonMovilidad.Application.Features.Vehicles.GetVehicleHistory
{
    internal sealed class GetVehicleHistoryQueryHandler : IQueryHandler<GetVehicleHistoryQuery, IEnumerable<HistoricalDataPointResponse>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly ISpeedCalculator _speedCalculator;

        public GetVehicleHistoryQueryHandler(ISqlConnectionFactory sqlConnectionFactory, ISpeedCalculator speedCalculator)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _speedCalculator = speedCalculator;
        }

        public async Task<Result<IEnumerable<HistoricalDataPointResponse>>> Handle(GetVehicleHistoryQuery request, CancellationToken cancellationToken)
        {
            //1. Crear la conexión a la base de datos
            using var connection = _sqlConnectionFactory.CreateConnection();

            //2. Definir la consulta SQL para obtener el historial del vehículo
            const string sql = """
                SELECT 
                    sr.timestamp AS "Timestamp",
                    sr.latitude AS "Latitude",
                    sr.longitude AS "Longitude",
                    sr.fuel_level AS "FuelLevel",
                    sr.temperature AS "Temperature"
                FROM 
                    sensor_readings sr
                WHERE 
                    sr.vehicle_id = @vehicleId
                    AND sr.timestamp BETWEEN @StartTime AND @EndTime
                ORDER BY 
                    sr.timestamp;
                """;

            //3. Ejecutar la consulta y mapear los resultados a la respuesta
            var readings = (await connection.QueryAsync<SensorReading>(sql, new
            {
                request.VehicleId,
                request.StartTime,
                request.EndTime
            })).ToList();

            if (readings.Count() < 2)
            {
                return Result.Failure<IEnumerable<HistoricalDataPointResponse>>(VehicleError.InsufficientData);
            }

            //4. Logica de negocio para calcular la velocidad
            var speed = _speedCalculator.CalculatorSpeedKph(readings).ToList();

            //5. Combinar los resultados para el modelo de respuesta
            //Se omite la primera lectura porque no tiene un punto anterior para calcular la velocidad
            var history = readings.Skip(1).Select((reading, index) => new HistoricalDataPointResponse
            {
                TimeStamp = reading.TimeStamp,
                FuelLevel = reading.FuelLevel,
                SpeedKmH = speed[index]
            });

            return Result.Success(history);
        }

        
    }
}
