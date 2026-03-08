namespace Application.DTOs._cuotaServicio.Request
{
    public class CuotaServicioRequest
    {
        public int NumeroCuota { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal MontoARS { get; set; }
        public int ServicioId { get; set; }
        public int CotizacionDolarId { get; set; }
    }
}
