using SimonMovilidad.Application.Abstractions.Messaging;

namespace SimonMovilidad.Application.Features.Users.LoginUser
{
    public record LoginUserQuery(string Email, string Password) : IQuery<string>;
}
