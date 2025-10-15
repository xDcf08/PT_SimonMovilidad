using SimonMovilidad.Domain.Abstractions;
using SimonMovilidad.Domain.Entities;

namespace SimonMovilidad.Application.Services
{
    internal sealed class SpeedCalculator : ISpeedCalculator
    {
        public IEnumerable<double> CalculatorSpeedKph(IReadOnlyList<SensorReading> readings)
        {
            if (readings.Count < 2)
            {
                yield break;
            }

            for (int i = 1; i < readings.Count; i++)
            {
                var previousReading = readings[i - 1];
                var currentReading = readings[i];

                var timeDifference = currentReading.TimeStamp - previousReading.TimeStamp;

                if (timeDifference.TotalHours <= 0)
                {
                    yield return 0.0;
                    continue;         
                }

                var distanceInKm = CalculateDistance(
                    (double)previousReading.Latitude, (double)previousReading.Longitude,
                    (double)currentReading.Latitude, (double)currentReading.Longitude);

                var speedInKph = distanceInKm / timeDifference.TotalHours;

                yield return speedInKph;
            }
        }

        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Radio de la Tierra en kilometros
            var φ1 = lat1 * Math.PI / 180; // φ, λ en radianes
            var φ2 = lat2 * Math.PI / 180;
            var Δφ = (lat2 - lat1) * Math.PI / 180;
            var Δλ = (lon2 - lon1) * Math.PI / 180;
            var a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                    Math.Cos(φ1) * Math.Cos(φ2) *
                    Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = R * c; // en kilometros
            return distance;
        }
    }
}
