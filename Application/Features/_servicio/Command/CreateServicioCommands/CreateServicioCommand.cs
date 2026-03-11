using Application.DTOs._servicio.Request;
using Application.DTOs._servicio.Response;
using MediatR;

namespace Application.Features._servicio.Command.CreateServicioCommands
{
    public record CreateServicioCommand(ServicioRequest Request) : IRequest<ServicioResponse>;
}
