using Application.DTOs._cuotaServicio.Response;
using Application.Wrappers;
using MediatR;

namespace Application.Features._cuotaServicio.Queries.GetAllCuotasServicioQueries
{
    public class GetAllCuotasServicioQuery : IRequest<PagedResponse<IEnumerable<CuotaServicioResponse>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllCuotasServicioQuery()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        public GetAllCuotasServicioQuery(GetAllCuotasServicioParameters parameters)
        {
            PageNumber = parameters.PageNumber == 0 ? 1 : parameters.PageNumber;
            PageSize = parameters.PageSize == 0 ? 10 : parameters.PageSize;
        }
    }
}
