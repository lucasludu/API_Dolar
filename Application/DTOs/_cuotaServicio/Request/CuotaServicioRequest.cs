namespace Application.DTOs._cuotaServicio.Request
{
    public class CuotaServicioRequest
    {
        public int NumeroCuota { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal MontoARS { get; set; }
        public decimal MontoUSD { get; set; }
        public int ServicioId { get; set; }
        public string TipoDolar { get; set; }
        public string DeterminaCuotaPor { get; set; }
    }
}
