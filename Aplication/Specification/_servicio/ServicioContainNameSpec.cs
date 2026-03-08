using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specification._servicio
{
    public class ServicioContainNameSpec : Specification<Servicio>
    {
        public ServicioContainNameSpec(string name)
        {
            Query.Where(s => s.Nombre.ToLower().Trim().Contains(name.ToLower().Trim()));
        }
    }
}
