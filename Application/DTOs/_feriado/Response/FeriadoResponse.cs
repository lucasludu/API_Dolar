using System.Text.Json.Serialization;

namespace Application.DTOs._feriado.Response
{
    public class FeriadoResponse
    {
        [JsonPropertyName("fecha")]
        public string Fecha { get; set; }

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }
    }
}
