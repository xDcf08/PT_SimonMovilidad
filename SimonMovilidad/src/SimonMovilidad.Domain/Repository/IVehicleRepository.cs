using SimonMovilidad.Domain.Entities;

namespace SimonMovilidad.Domain.Repository
{
    public interface IVehicleRepository
    {
        Task<Vehicle?> GetByDeviceIdAsync(string deviceId, CancellationToken cancellationToken = default);
        Task<Vehicle?> GetByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken = default);
        Task AddAsync(Vehicle vehicle);
    }
}
