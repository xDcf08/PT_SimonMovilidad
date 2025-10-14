namespace SimonMovilidad.Application.Abstractions.PasswordHash
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string storedHashString);
    }
}
