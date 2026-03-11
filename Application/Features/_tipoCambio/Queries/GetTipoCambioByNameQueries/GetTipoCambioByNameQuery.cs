using Application.DTOs._tipoDolar.Response;
using MediatR;

namespace Application.Features._tipoCambio.Queries.GetTipoCambioByNameQueries
{
    public record GetTipoCambioByNameQuery(string Name) : IRequest<TipoDolarResponse>;
}
