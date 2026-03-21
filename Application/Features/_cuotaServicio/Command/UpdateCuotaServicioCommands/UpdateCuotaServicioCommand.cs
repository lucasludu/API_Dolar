using Application.DTOs._cuotaServicio.Request;
using Application.DTOs._cuotaServicio.Response;
using Application.Wrappers;
using MediatR;

namespace Application.Features._cuotaServicio.Command.UpdateCuotaServicioCommands
{
    public record UpdateCuotaServicioCommand(int Id, CuotaServicioRequest CuotaServicioDto) : IRequest<Response<CuotaServicioResponse>>;
}
