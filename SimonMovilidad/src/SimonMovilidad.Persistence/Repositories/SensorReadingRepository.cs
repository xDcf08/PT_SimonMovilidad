using SimonMovilidad.Domain.Entities;
using SimonMovilidad.Domain.Repository;

namespace SimonMovilidad.Persistence.Repositories
{
    internal sealed class SensorReadingRepository : Repository<SensorReading>, ISensorReadingRepository
    {
        public SensorReadingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task AddAsync(SensorReading sensorReading)
        {
            await _context.Set<SensorReading>().AddAsync(sensorReading);
        }
    }
}
