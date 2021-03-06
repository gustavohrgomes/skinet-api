using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly StoreContext _context;
        private readonly DbSet<TEntity> _dbContext;

        public Repository(StoreContext context)
        {
            _context = context;
            _dbContext = context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            _dbContext.Add(entity);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Remove(entity);
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbContext.FindAsync(id);
        }

        public async Task<IReadOnlyList<TEntity>> ListAllAsync()
        {
            return await _dbContext.ToListAsync();
        }

        public async Task<TEntity> GetEntityWithSpec(ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).CountAsync();
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
        {
            return SpecificationEvaluator<TEntity>.GetQuery(_dbContext.AsQueryable(), specification);
        }
    }
}