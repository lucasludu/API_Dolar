using Application.DTOs._cuotaServicio.Response;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using Domain.Entities;

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

    public class GetAllCuotasServicioQueryHandler : IRequestHandler<GetAllCuotasServicioQuery, PagedResponse<IEnumerable<CuotaServicioResponse>>>
    {
        private readonly IRepositoryAsync<CuotaServicio> _repository;
        private readonly IMapper _mapper;

        public GetAllCuotasServicioQueryHandler(IRepositoryAsync<CuotaServicio> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<CuotaServicioResponse>>> Handle(GetAllCuotasServicioQuery request, CancellationToken cancellationToken)
        {
            var pagedCuotas = await _repository.GetPagedResponseAsync(request.PageNumber, request.PageSize);
            var totalRecords = await _repository.CountAsync(cancellationToken);
            var cuotasViewModel = _mapper.Map<IEnumerable<CuotaServicioResponse>>(pagedCuotas);
            return new PagedResponse<IEnumerable<CuotaServicioResponse>>(cuotasViewModel, request.PageNumber, request.PageSize, totalRecords);
        }
    }
}
