using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entity;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specification;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _DbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _DbContext = dbContext;
        }
        #region Static
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
         
            return await _DbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            
            return await _DbContext.Set<T>().FindAsync(id);
        } 
        #endregion

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {

            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvalutor<T>.GetQuery(_DbContext.Set<T>(), spec);
        }

        public async Task<int> GetCountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task AddAsync(T entity)
            => await _DbContext.AddAsync(entity);

        public void Update(T entity)
            => _DbContext.Update(entity);

        public void Delete(T entity)
            => _DbContext.Remove(entity);
    }
}
