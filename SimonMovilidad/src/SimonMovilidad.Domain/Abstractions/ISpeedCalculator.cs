using SimonMovilidad.Domain.Entities;

namespace SimonMovilidad.Domain.Abstractions
{
    public interface ISpeedCalculator
    {
        IEnumerable<double> CalculatorSpeedKph(IReadOnlyList<SensorReading> readings);
    }
}
