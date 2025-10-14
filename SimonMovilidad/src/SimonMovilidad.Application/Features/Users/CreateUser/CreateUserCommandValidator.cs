using FluentValidation;

namespace SimonMovilidad.Application.Features.Users.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("El email es requerido.")
                .EmailAddress().WithMessage("Formato inválido del email.")
                .NotNull();

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("La contraseña es requerida.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.")
                .NotNull();

            RuleFor(u => u.Role)
                .IsInEnum().WithMessage("El rol proporcionado no es válido.")
                .NotNull();

        }
    }
}
