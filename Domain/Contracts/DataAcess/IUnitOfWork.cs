using Core.Entities;
using System;
using System.Threading.Tasks;

namespace Core.Contracts.DataAcess
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class, BaseEntity;
        Task<int> CommitAsync();
    }
}
