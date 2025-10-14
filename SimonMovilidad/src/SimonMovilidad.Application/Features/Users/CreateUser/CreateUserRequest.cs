using SimonMovilidad.Domain.Enums;

namespace SimonMovilidad.Application.Features.Users.CreateUser
{
    public record CreateUserRequest(string Email, string Password, RoleEnum Role);
}
