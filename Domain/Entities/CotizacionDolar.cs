using Domain.Common;

namespace Domain.Entities
{
    public class CotizacionDolar : AuditableBaseEntity
    {
        public DateTime Fecha { get; private set; }
        public decimal? Compra { get; private set; }
        public decimal? Venta { get; private set; }

        public string TipoDolar { get; private set; }

        protected CotizacionDolar() { } // EF Core

        public CotizacionDolar(DateTime fecha, string tipoDolar, decimal? compra = null, decimal? venta = null)
        {
            Fecha = fecha;
            TipoDolar = tipoDolar;
            Compra = compra;
            Venta = venta;
        }
    }
}
