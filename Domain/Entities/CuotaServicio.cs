using Domain.Common;

namespace Domain.Entities
{
    public class CuotaServicio : AuditableBaseEntity
    {
        public int NumeroCuota { get; private set; }
        public DateTime FechaPago { get; private set; }
        public decimal MontoARS { get; private set; }
        public decimal MontoUSD { get; private set; }

        public int ServicioId { get; private set; }
        public Servicio Servicio { get; private set; }

        public int CotizacionDolarId { get; private set; }
        public CotizacionDolar CotizacionDolar { get; private set; }

        protected CuotaServicio() { } // EF Core

        public CuotaServicio(int numeroCuota, DateTime fechaPago, decimal montoARS, decimal montoUSD, int servicioId, int cotizacionDolarId)
        {
            NumeroCuota = numeroCuota;
            FechaPago = fechaPago;
            MontoARS = montoARS;
            MontoUSD = montoUSD;
            ServicioId = servicioId;
            CotizacionDolarId = cotizacionDolarId;
        }

        public void Update(int numeroCuota, DateTime fechaPago, decimal montoARS, decimal montoUSD, int servicioId, int cotizacionDolarId)
        {
            NumeroCuota = numeroCuota;
            FechaPago = fechaPago;
            MontoARS = montoARS;
            MontoUSD = montoUSD;
            ServicioId = servicioId;
            CotizacionDolarId = cotizacionDolarId;
        }
    }
}
