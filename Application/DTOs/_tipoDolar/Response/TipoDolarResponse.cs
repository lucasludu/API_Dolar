namespace Application.DTOs._tipoDolar.Response
{
    public class TipoDolarResponse
    {
        public string Moneda { get; set; }
        public string Casa { get; set; }
        public decimal? Compra { get; set; }
        public decimal? Venta { get; set; }
        public string? FechaActualizacion { get; set; }
    }
}
