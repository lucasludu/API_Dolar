using Domain.Common;

namespace Domain.Entities
{
    public class CuotaServicio : AuditableBaseEntity
    {
        public int NumeroCuota { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal MontoARS { get; set; }
        public decimal MontoUSD { get; set; }

        public int ServicioId { get; set; }
        public Servicio Servicio { get; set; }

        public int CotizacionDolarId { get; set; }
        public CotizacionDolar CotizacionDolar { get; set; }

   
    }
}
