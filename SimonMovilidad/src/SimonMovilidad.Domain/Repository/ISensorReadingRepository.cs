using SimonMovilidad.Domain.Entities;

namespace SimonMovilidad.Domain.Repository
{
    public interface ISensorReadingRepository
    {
        Task AddAsync(SensorReading sensorReading);
    }
}
