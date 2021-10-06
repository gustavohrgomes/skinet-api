﻿using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GeByIdAsync(int id);
        Task<IReadOnlyList<TEntity>> ListAllAsync();
    }
}