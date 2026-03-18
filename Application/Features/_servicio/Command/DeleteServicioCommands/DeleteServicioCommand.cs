using Application.Wrappers;
using MediatR;

namespace Application.Features._servicio.Command.DeleteServicioCommands
{
    public record DeleteServicioCommand(int id) : IRequest<Response<bool>>;
}
