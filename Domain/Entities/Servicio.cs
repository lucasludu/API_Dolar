using Domain.Common;

namespace Domain.Entities
{
    public class Servicio : AuditableBaseEntity
    {
        public string Nombre { get; private set; } 
        public DateTime FechaInicio { get; private set; }
        public int? CantidadCuotas { get; private set; }
        public bool Activo { get; private set; } = true;


        public ICollection<CuotaServicio>? Cuotas { get; private set; }

        protected Servicio() { } // EF Core

        public Servicio(string nombre, DateTime fechaInicio, int? cantidadCuotas, bool activo = true)
        {
            Nombre = nombre;
            FechaInicio = fechaInicio;
            CantidadCuotas = cantidadCuotas;
            Activo = activo;
        }
    }
}
