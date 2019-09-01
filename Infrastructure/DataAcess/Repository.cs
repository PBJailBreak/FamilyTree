using Core.Contracts.DataAcess;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.DataAcess
{
    public class Repository<T> : IRepository<T> where T : class, BaseEntity
    {
        private readonly FamilyTreeDbContext dbContext;

        public Repository(FamilyTreeDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<T> InsertAsync(T entity)
        {
            return (await this.dbContext.Set<T>().AddAsync(entity)).Entity;
        }

        public async Task<T> GetAsync(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null
        )
        {
            IQueryable<T> query = this.dbContext.Set<T>();

            if (include != null) query = include(query);
            if (predicate != null) query = query.Where(predicate);

            return await query.FirstOrDefaultAsync();
        }
    }
}
