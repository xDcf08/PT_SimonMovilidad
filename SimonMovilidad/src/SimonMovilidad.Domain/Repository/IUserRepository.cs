using SimonMovilidad.Domain.Entities;

namespace SimonMovilidad.Domain.Repository
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task AddAsync(User user);
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
