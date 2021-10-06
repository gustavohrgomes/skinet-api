using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<TEntity> GeByIdAsync(int id)
        {
            return await _dbContext.FindAsync(id);
        }

        public async Task<IReadOnlyList<TEntity>> ListAllAsync()
        {
            return await _dbContext.ToListAsync();
        }
    }
}
