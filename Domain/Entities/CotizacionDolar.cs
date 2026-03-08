using Domain.Common;

namespace Domain.Entities
{
    public class CotizacionDolar : AuditableBaseEntity
    {
        public DateTime Fecha { get; set; }
        public decimal? Compra { get; set; }
        public decimal? Venta { get; set; }

        public int TipoDolarId { get; set; }
        public TipoDolar TipoDolar { get; set; }
    }
}
