using Application.DTOs._servicio.Response;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;

namespace Application.Features._servicio.Queries.GetAllServiciosQueries
{
    public class GetAllServiciosQuery : IRequest<PagedResponse<IEnumerable<ServicioResponse>>>, ICacheable
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public string CacheKey => $"GetAllServicios_{PageNumber}_{PageSize}";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public GetAllServiciosQuery()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        public GetAllServiciosQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber == 0 ? 1 : pageNumber;
            PageSize = pageSize == 0 ? 10 : pageSize;
        }
    }
}
