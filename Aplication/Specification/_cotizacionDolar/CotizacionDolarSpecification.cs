using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specification._cotizacionDolar
{
    public class CotizacionDolarSpecification : Specification<CotizacionDolar>
    {
        public CotizacionDolarSpecification(string tipo, DateTime fecha)
        {
            Query
                .Where(a => a.TipoDolar.Nombre.ToLower() == tipo.ToLower() && a.Fecha == fecha);
        }
    }
}
