using Domain.Common;

namespace Domain.Entities
{
    public class Servicio : AuditableBaseEntity
    {
        public string Nombre { get; set; } 
        public DateTime FechaInicio { get; set; }
        public int? CantidadCuotas { get; set; }
        public bool Activo { get; set; } = true;


        public ICollection<CuotaServicio>? Cuotas { get; set; }
    }
}
