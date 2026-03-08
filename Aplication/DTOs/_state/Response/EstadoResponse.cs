using System.Text.Json.Serialization;

namespace Application.DTOs._state.Response
{
    public class EstadoResponse
    {
        [JsonPropertyName("estado")]
        public string Estado { get; set; }
    }
}
