using Application.DTOs._cuotaServicio.Request;
using MediatR;

namespace Application.Features._cuotaServicio.Command.UpdateCuotaServicioCommands
{
    public record UpdateCuotaServicioCommand(int Id, CuotaServicioRequest CuotaServicioDto) : IRequest<bool>;
}
