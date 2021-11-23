using Domain.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Service.Base
{
    public interface IService<T> : IDisposable
    {
        Task<Resultado<IEnumerable<T>>> ListAsync();
        Task<Resultado<T>> GetAsync(int code);
        Task<Resultado<IEnumerable<T>>> SelectAsync(T entity);
        Task<Resultado<T>> CreateAsync(T entity);
        Task<Resultado<T>> UpdateAsync(T entity);
        Task<Resultado<T>> DeleteAsync(int code/*, int user*/);
    }
}
