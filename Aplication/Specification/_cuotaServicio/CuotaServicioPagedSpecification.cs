using Application.Feautures._cuotaServicio.Queries.GetAllCuotasServicioQueries;
using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specification._cuotaServicio
{
    public class CuotaServicioPagedSpecification : Specification<CuotaServicio>
    {
        public CuotaServicioPagedSpecification(GetAllCuotasServicioParameters parameters)
        {
            Query.Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .Include(a => a.Servicio)
                .Include(a => a.CotizacionDolar)
                .Include(a => a.CotizacionDolar.TipoDolar);
        }
    }
}
