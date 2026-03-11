using MediatR;

namespace Application.Features._cuotaServicio.Command.DeleteCuotaServicioCommands
{
    public record DeleteCuotaServicioCommand(int Id) : IRequest<bool>;
}
