using SimonMovilidad.Domain.Entities;

namespace SimonMovilidad.Domain.Repository
{
    public interface IAlertRepository
    {
        Task AddAsync(Alert alert);
    }
}
