using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repository.Base
{
    public interface IRepository<T> : IDisposable
    {
        Task<IEnumerable<T>> ListAsync();
        Task<T> GetAsync(int code);
        Task<IEnumerable<T>> SelectAsync(T entity);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(int code/*, int user*/);
    }
}
