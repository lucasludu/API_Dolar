using FluentValidation;

namespace Application.Features._servicio.Command.CreateServicioCommands
{
    public class CreateServicioCommandValidator : AbstractValidator<CreateServicioCommand>
    {
        public CreateServicioCommandValidator()
        {
            RuleFor(p => p.Request.Nombre)
                .NotEmpty().WithMessage("{PropertyName} no puede estar vacío.")
                .MaximumLength(100).WithMessage("{PropertyName} no debe exceder los 100 caracteres.");

            RuleFor(p => p.Request.FechaInicio)
                .NotEmpty().WithMessage("{PropertyName} es requerida.");

            RuleFor(p => p.Request.CantidadCuotas)
                .GreaterThan(0).WithMessage("{PropertyName} debe ser mayor a 0.")
                .When(p => p.Request.CantidadCuotas.HasValue);
        }
    }
}
