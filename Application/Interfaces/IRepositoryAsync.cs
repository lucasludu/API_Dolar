using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRepositoryAsync<T> : IRepositoryBase<T> where T : class
    {
        Task<IReadOnlyList<T>> GetPagedResponseAsync(int pageNumber, int pageSize);
    }

    public interface IReadRepositoryAsync<T> : IReadRepositoryBase<T> where T : class 
    { 
    }

}
