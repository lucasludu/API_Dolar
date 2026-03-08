using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specification._tipoCambio
{
    public class TipoDolarByNameSpec : Specification<TipoDolar>
    {
        public TipoDolarByNameSpec(string name)
        {
            Query.Where(td => td.Nombre.ToLower().Trim().Equals(name.ToLower().Trim()));
        }
    }
}
