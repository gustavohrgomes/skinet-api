using Core.Entities;
using Core.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GeByIdAsync(int id);
        Task<IReadOnlyList<TEntity>> ListAllAsync();
        Task<TEntity> GetEntityWithSpec(ISpecification<TEntity> specification);
        Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> specification);
        Task<int> CountAsync(ISpecification<TEntity> specification);
    }
}