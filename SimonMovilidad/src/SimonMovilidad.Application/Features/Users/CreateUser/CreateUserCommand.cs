using SimonMovilidad.Application.Abstractions.Messaging;
using SimonMovilidad.Domain.Enums;

namespace SimonMovilidad.Application.Features.Users.CreateUser
{
    public record CreateUserCommand(string Email, string Password, RoleEnum Role) : ICommand<Guid>;
}
