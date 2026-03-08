using System.Text.Json.Serialization;

namespace Application.DTOs._cotizaciones.Response
{
    public class CotizacionesResponse
    {
        public int Id { get; set; }
        [JsonPropertyName("moneda")]
        public string Moneda { get; set; }

        [JsonPropertyName("casa")]
        public string Casa { get; set; }

        [JsonPropertyName("fecha")]
        public string Fecha { get; set; }

        [JsonPropertyName("compra")]
        public decimal? Compra { get; set; }  // ? Nullable

        [JsonPropertyName("venta")]
        public decimal? Venta { get; set; }   // ? Nullable

        [JsonPropertyName("fechaActualizacion")]
        public string FechaActualizacion { get; set; }
    }
}
