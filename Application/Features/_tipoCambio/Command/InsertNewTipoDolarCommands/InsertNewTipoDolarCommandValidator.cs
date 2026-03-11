using FluentValidation;
using Application.Features._tipoCambio.Command.InsertNewTipoDolarCommands;

namespace Application.Features._tipoCambio.Command.InsertNewTipoDolarCommands
{
    public class InsertNewTipoDolarCommandValidator : AbstractValidator<InsertNewTipoDolarCommand>
    {
        public InsertNewTipoDolarCommandValidator()
        {
            RuleFor(p => p.Request.Nombre)
                .NotEmpty().WithMessage("{PropertyName} no puede estar vacío.")
                .MaximumLength(50).WithMessage("{PropertyName} no debe exceder los 50 caracteres.");
        }
    }
}
