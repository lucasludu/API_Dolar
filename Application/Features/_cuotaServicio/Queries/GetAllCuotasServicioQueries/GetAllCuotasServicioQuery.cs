using Application.DTOs._cuotaServicio.Response;
using Application.Wrappers;
using MediatR;

namespace Application.Features._cuotaServicio.Queries.GetAllCuotasServicioQueries
{
    public class GetAllCuotasServicioQuery : IRequest<PagedResponse<IEnumerable<CuotaServicioResponse>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? ServicioId { get; set; }

        public GetAllCuotasServicioQuery(int? servicioId = null)
        {
            PageNumber = 1;
            PageSize = 10;
            ServicioId = servicioId;
        }

        public GetAllCuotasServicioQuery(GetAllCuotasServicioParameters parameters)
        {
            PageNumber = parameters.PageNumber == 0 ? 1 : parameters.PageNumber;
            PageSize = parameters.PageSize == 0 ? 10 : parameters.PageSize;
            ServicioId = parameters.ServicioId;
        }
    }
}
