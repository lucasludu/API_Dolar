using Application.DTOs._servicio.Request;
using Application.Wrappers;
using MediatR;

namespace Application.Features._servicio.Command.UpdateServicioCommands
{
    public record UpdateServicioCommand(int id, ServicioRequest Request) : IRequest<Response<bool>>;
}
