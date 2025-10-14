using SimonMovilidad.Domain.Entities;
using SimonMovilidad.Domain.Repository;

namespace SimonMovilidad.Persistence.Repositories
{
    internal sealed class AlertRepository : Repository<Alert>, IAlertRepository
    {
        public AlertRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task AddAsync(Alert alert)
        {
            await _context.Set<Alert>().AddAsync(alert);
        }
    }
}
