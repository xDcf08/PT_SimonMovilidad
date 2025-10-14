using SimonMovilidad.Domain.Entities;

namespace SimonMovilidad.Application.Abstractions.Authentication
{
    public interface IJwtProvider
    {
        string Generate(User user);
    }
}
