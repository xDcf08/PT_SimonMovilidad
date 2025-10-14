using SimonMovilidad.Application.Abstractions.Messaging;

namespace SimonMovilidad.Application.Features.Users.GetUserById
{
    public record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;
}
