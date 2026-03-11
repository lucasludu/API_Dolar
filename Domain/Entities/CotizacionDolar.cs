using Domain.Common;

namespace Domain.Entities
{
    public class CotizacionDolar : AuditableBaseEntity
    {
        public DateTime Fecha { get; private set; }
        public decimal? Compra { get; private set; }
        public decimal? Venta { get; private set; }

        public int TipoDolarId { get; private set; }
        public TipoDolar TipoDolar { get; private set; }

        protected CotizacionDolar() { } // EF Core

        public CotizacionDolar(DateTime fecha, int tipoDolarId, decimal? compra = null, decimal? venta = null)
        {
            Fecha = fecha;
            TipoDolarId = tipoDolarId;
            Compra = compra;
            Venta = venta;
        }
    }
}
