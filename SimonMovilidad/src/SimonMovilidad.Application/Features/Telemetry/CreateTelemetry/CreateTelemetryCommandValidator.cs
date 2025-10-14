using FluentValidation;

namespace SimonMovilidad.Application.Features.Telemetry.CreateTelemetry
{
    public class CreateTelemetryCommandValidator : AbstractValidator<CreateTelemetryCommand>
    {
        public CreateTelemetryCommandValidator()
        {
            RuleFor(x => x.DeviceId)
            .NotEmpty().WithMessage("El DeviceId es obligatorio.")
            // Esta expresión regular valida un formato como "DEV-A1B2-C3D4"
            .Matches("^[A-Z]{3}-[A-Z0-9]{4}-[A-Z0-9]{4}$")
            .WithMessage("El formato del DeviceId debe ser 'XXX-XXXX-XXXX'.");

            RuleFor(x => x.Latitude)
                // La latitud debe estar entre -90 y 90
                .InclusiveBetween(-90, 90).WithMessage("La latitud no es válida.");

            RuleFor(x => x.Longitude)
                // La longitud debe estar entre -180 y 180
                .InclusiveBetween(-180, 180).WithMessage("La longitud no es válida.");

            RuleFor(x => x.FuelLevel)
                .GreaterThanOrEqualTo(0).WithMessage("El nivel de combustible no puede ser negativo.");

            RuleFor(x => x.Timestamp)
                // Evita que se envíen fechas futuras
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("El timestamp no puede ser una fecha futura.");
        }
    }
}
