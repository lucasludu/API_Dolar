using Application.DTOs._cuotaServicio.Request;
using Application.DTOs._cuotaServicio.Response;
using Application.Wrappers;
using MediatR;

namespace Application.Features._cuotaServicio.Command.CargarCuotasServicioCommands
{
    public record CargarCuotasServicioCommand(CuotaServicioRequest Request) : IRequest<Response<CuotaServicioResponse>>;
}
