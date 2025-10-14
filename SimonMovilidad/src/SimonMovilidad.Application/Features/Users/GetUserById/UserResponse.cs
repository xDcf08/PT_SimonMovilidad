using SimonMovilidad.Domain.Enums;

namespace SimonMovilidad.Application.Features.Users.GetUserById
{
    public sealed class UserResponse
    {
        public Guid Id { get; init; }
        public required string Email { get; init; }
        public required RoleEnum Role { get; init; }
    }
}
