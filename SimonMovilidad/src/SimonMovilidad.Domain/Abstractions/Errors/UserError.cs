namespace SimonMovilidad.Domain.Abstractions.Errors
{
    public static class UserError
    {
        public static Error AlreadyExists = new("User.AlreadyExists", "El correo ingresado ya se encuentra registrado");
        public static Error NotFound = new("User.NotFound", "El usuario no fue encontrado");
        public static Error InvalidCredentials = new("User.InvalidCredentials", "Credenciales inválidas");
    }
}
