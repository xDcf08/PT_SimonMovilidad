using FluentValidation;

namespace SimonMovilidad.Application.Features.Vehicles.CreateVehicle
{
    public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
    {
        public CreateVehicleCommandValidator()
        {
            RuleFor(x => x.DeviceId)
                .NotEmpty().WithMessage("DeviceId es requerido.")
                .MaximumLength(100).WithMessage("DeviceId no debe exceder los 100 caracteres.");

            RuleFor(x => x.LicensePlate)
                .NotEmpty().WithMessage("La Matrícula es requerida.")
                .MaximumLength(20).WithMessage("La Matrícula no debe exceder los 20 caracteres.")
                .Matches("^[A-Z0-9]+$").WithMessage("La Matrícula solo debe contener letras mayúsculas y números.");

            RuleFor(x => x.AvgConsumption)
                .GreaterThan(0).WithMessage("El Consumo Promedio (litros/hora) debe ser mayor que 0.");
        }
    }
}
