using Application.DTOs._cuotaServicio.Response;
using Application.Interfaces;
using Application.Wrappers;
using Application.Specification._cuotaServicio;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Ardalis.Specification;

namespace Application.Features._cuotaServicio.Queries.GetAllCuotasServicioQueries
{
    public class GetAllCuotasServicioQueryHandler : IRequestHandler<GetAllCuotasServicioQuery, PagedResponse<IEnumerable<CuotaServicioResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCuotasServicioQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<CuotaServicioResponse>>> Handle(GetAllCuotasServicioQuery request, CancellationToken cancellationToken)
        {
            var parameters = new GetAllCuotasServicioParameters
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                ServicioId = request.ServicioId
            };

            var pagedCuotas = await _unitOfWork.RepositoryAsync<CuotaServicio>().ListAsync(new CuotaServicioPagedSpecification(parameters), cancellationToken);

            int totalRecords = (request.ServicioId.HasValue)
                ? await _unitOfWork.RepositoryAsync<CuotaServicio>().CountAsync(new CuotaServicioPagedSpecification(parameters), cancellationToken)
                : await _unitOfWork.RepositoryAsync<CuotaServicio>().CountAsync(cancellationToken); 

            var cuotasViewModel = _mapper.Map<IEnumerable<CuotaServicioResponse>>(pagedCuotas);
            return new PagedResponse<IEnumerable<CuotaServicioResponse>>(cuotasViewModel, request.PageNumber, request.PageSize, totalRecords);
        }
    }
}
