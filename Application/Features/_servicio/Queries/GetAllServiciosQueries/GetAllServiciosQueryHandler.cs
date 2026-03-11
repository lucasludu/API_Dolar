using Application.DTOs._servicio.Response;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features._servicio.Queries.GetAllServiciosQueries
{
    public class GetAllServiciosQueryHandler : IRequestHandler<GetAllServiciosQuery, PagedResponse<IEnumerable<ServicioResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllServiciosQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<ServicioResponse>>> Handle(GetAllServiciosQuery request, CancellationToken cancellationToken)
        {
            var pagedServicios = await _unitOfWork.RepositoryAsync<Servicio>().GetPagedResponseAsync(request.PageNumber, request.PageSize);
            var totalRecords = await _unitOfWork.RepositoryAsync<Servicio>().CountAsync(cancellationToken);
            var serviciosViewModel = _mapper.Map<IEnumerable<ServicioResponse>>(pagedServicios);
            return new PagedResponse<IEnumerable<ServicioResponse>>(serviciosViewModel, request.PageNumber, request.PageSize, totalRecords);
        }
    }
}
