using Core.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Contracts.DataAcess
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> InsertAsync(T entity);

        // include param violates the "Onion" architecture, but ¯\_(ツ)_/¯
        Task<T> GetAsync(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null
        );
    }
}
