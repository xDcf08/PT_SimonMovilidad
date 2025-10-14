using SimonMovilidad.Application.Services;
using SimonMovilidad.Domain.Abstractions;
using SimonMovilidad.Domain.Entities;
using Xunit;

namespace SimonMovilidad.Application.UnitTest.Vehicles
{
    public class SpeedCalculatorTests
    {
        private readonly ISpeedCalculator _speedCalculatorMock;

        public SpeedCalculatorTests()
        {
            _speedCalculatorMock = new SpeedCalculator();
        }

        [Fact]
        public void CalculateSpeedsKph_GivenTwoPointsOneKmApartOverTenMinutes_ShouldReturnSpeedOf6Kph()
        {
            //ARRANGE
            var startTime = DateTime.UtcNow;
            var endTime = startTime.AddMinutes(10);

            var readings = new List<SensorReading>
            {
                new SensorReading{TimeStamp = startTime, Latitude = 4.5709m, Longitude = -74.2973m},
                new SensorReading{TimeStamp = endTime, Latitude = 4.5799m, Longitude = -74.2973m}
            };

            //ACT
            var speed = _speedCalculatorMock.CalculatorSpeedKph(readings).ToList();

            //ASSERT
            Assert.Single(speed);
            Assert.InRange(speed[0], 5.9, 6.1);
        }
    }
}
