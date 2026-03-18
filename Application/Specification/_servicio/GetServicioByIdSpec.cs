using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specification._servicio
{
    public class GetServicioByIdSpec : Specification<Servicio>
    {
        public GetServicioByIdSpec(int id)
        {
            Query.Where(s => s.Id == id)
                .AsNoTracking();
        }
    }
}
