namespace Application.DTOs._servicio.Response
{
    public class ServicioResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public int? CantidadCuotas { get; set; }
        public bool Activo { get; set; } = true;
    }
}
