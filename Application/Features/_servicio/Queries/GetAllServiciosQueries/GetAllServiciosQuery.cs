using Application.DTOs._servicio.Response;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using Domain.Entities;

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

    public class GetAllServiciosQueryHandler : IRequestHandler<GetAllServiciosQuery, PagedResponse<IEnumerable<ServicioResponse>>>
    {
        private readonly IRepositoryAsync<Servicio> _repository;
        private readonly IMapper _mapper;

        public GetAllServiciosQueryHandler(IRepositoryAsync<Servicio> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<ServicioResponse>>> Handle(GetAllServiciosQuery request, CancellationToken cancellationToken)
        {
            var pagedServicios = await _repository.GetPagedResponseAsync(request.PageNumber, request.PageSize);
            var totalRecords = await _repository.CountAsync(cancellationToken);
            var serviciosViewModel = _mapper.Map<IEnumerable<ServicioResponse>>(pagedServicios);
            return new PagedResponse<IEnumerable<ServicioResponse>>(serviciosViewModel, request.PageNumber, request.PageSize, totalRecords);
        }
    }
}
