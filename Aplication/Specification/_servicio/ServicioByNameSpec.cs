using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specification._servicio
{
    public class ServicioByNameSpec : Specification<Servicio>
    {
        public ServicioByNameSpec(string name)
        {
            Query.Where(s => s.Nombre.ToLower().Equals(name.ToLower()) && s.Activo == true).AsNoTracking();
        }
    }
}
