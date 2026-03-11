using Domain.Common;

namespace Domain.Entities
{
    public class TipoDolar : AuditableBaseEntity
    {
        public string Nombre { get; private set; }

        protected TipoDolar() { } // EF Core

        public TipoDolar(string nombre)
        {
            Nombre = nombre;
        }
    }
}
