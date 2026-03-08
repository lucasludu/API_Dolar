using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specification._tipoCambio
{
    public class TipoDolarContainNameSpec : Specification<TipoDolar>
    {
        public TipoDolarContainNameSpec(string name)
        {
            Query.Where(td => td.Nombre.ToLower().Trim().Contains(name.ToLower().Trim()));
        }
    }
}
