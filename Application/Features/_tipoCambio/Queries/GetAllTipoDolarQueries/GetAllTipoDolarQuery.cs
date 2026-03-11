using Application.DTOs._tipoDolar.Response;
using MediatR;

namespace Application.Features._tipoCambio.Queries.GetAllTipoDolarQueries
{
    public record GetAllTipoDolarQuery : IRequest<IEnumerable<TipoDolarResponse>>;
}
