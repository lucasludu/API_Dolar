using Application.DTOs._cuotaServicio.Request;
using Application.DTOs._cuotaServicio.Response;
using MediatR;

namespace Application.Features._cuotaServicio.Command.CargarCuotasServicioCommands
{
    public record CargarCuotasServicioCommand(CuotaServicioRequest Request) : IRequest<CuotaServicioResponse>;
}
