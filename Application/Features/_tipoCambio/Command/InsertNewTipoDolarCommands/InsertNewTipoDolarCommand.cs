using Application.DTOs._tipoDolar.Request;
using Application.DTOs._tipoDolar.Response;
using MediatR;

namespace Application.Features._tipoCambio.Command.InsertNewTipoDolarCommands
{
    public record InsertNewTipoDolarCommand(TipoDolarRequest Request) : IRequest<TipoDolarResponse>;
}
