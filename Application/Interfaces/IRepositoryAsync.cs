using Application.DTOs._cotizaciones.Response;
using Ardalis.Specification;

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
