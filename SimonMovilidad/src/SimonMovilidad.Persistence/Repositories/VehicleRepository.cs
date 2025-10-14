using Microsoft.EntityFrameworkCore;
using SimonMovilidad.Domain.Entities;
using SimonMovilidad.Domain.Repository;

namespace SimonMovilidad.Persistence.Repositories
{
    internal sealed class VehicleRepository : Repository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            await _context.Set<Vehicle>().AddAsync(vehicle);
        }

        public async Task<Vehicle?> GetByDeviceIdAsync(string deviceId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Vehicle>()
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.DeviceId == deviceId, cancellationToken);
        }

        public async Task<Vehicle?> GetByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Vehicle>()
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.LicensePlate == licensePlate, cancellationToken);
        }
    }
}
