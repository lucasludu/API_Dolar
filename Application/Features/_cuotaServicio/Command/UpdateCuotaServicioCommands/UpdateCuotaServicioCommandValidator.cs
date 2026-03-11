using FluentValidation;
using Application.Features._cuotaServicio.Command.UpdateCuotaServicioCommands;

namespace Application.Features._cuotaServicio.Command.UpdateCuotaServicioCommands
{
    public class UpdateCuotaServicioCommandValidator : AbstractValidator<UpdateCuotaServicioCommand>
    {
        public UpdateCuotaServicioCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} es requerido.");

            RuleFor(p => p.CuotaServicioDto.NumeroCuota)
                .GreaterThan(0).WithMessage("{PropertyName} debe ser mayor a 0.");

            RuleFor(p => p.CuotaServicioDto.MontoARS)
                .GreaterThan(0).WithMessage("{PropertyName} debe ser mayor a 0.");

            RuleFor(p => p.CuotaServicioDto.FechaPago)
                .NotEmpty().WithMessage("{PropertyName} es requerida.");
        }
    }
}
