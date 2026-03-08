using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specification._cuotaServicio
{
    public class CuotaServicioSpecification : Specification<CuotaServicio>
    {
        public CuotaServicioSpecification(int servicioId, int numeroCuota)
        {
            Query
                .Where(cs => cs.ServicioId == servicioId && cs.NumeroCuota == numeroCuota)
                .Include(cs => cs.Servicio)
                .Include(cs => cs.CotizacionDolar)
                .Include(cs => cs.CotizacionDolar.TipoDolar)
                .AsNoTracking();
        }
    }
}
