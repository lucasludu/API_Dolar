using Application.DTOs._servicio.Response;
using MediatR;

namespace Application.Features._servicio.Queries.GetServicioByNameQueries
{
    public record GetServicioByNameQuery(string Name) : IRequest<ServicioResponse>;
}
