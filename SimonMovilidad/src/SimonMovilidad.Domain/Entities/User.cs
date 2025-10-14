using SimonMovilidad.Domain.Enums;

namespace SimonMovilidad.Domain.Entities
{
    public sealed class User
    {
        public Guid Id { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
        public required RoleEnum Role { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
