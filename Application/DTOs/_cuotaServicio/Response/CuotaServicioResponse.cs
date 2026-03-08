namespace Application.DTOs._cuotaServicio.Response
{
    public class CuotaServicioResponse
    {
        public int Id { get; set; }
        public int NumeroCuota { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal MontoARS { get; set; }
        public decimal MontoUSD { get; set; }
        public string Servicio { get; set; }
        public string DolarVenta { get; set; }
        public string DolarCompra { get; set; }
        public string TipoDolar { get; set; }
    }
}
