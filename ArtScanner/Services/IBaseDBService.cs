using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ArtScanner.Models.Entities;

namespace ArtScanner.Services
{
    interface IBaseDBService
    {
        Task<int> DeleteAllAsync<TEntity>(IEnumerable<TEntity> items)
          where TEntity : BaseEntity, new();

        Task<TEntity> GetAsync<TEntity>(long id)
            where TEntity : BaseEntity, new();

        Task Add<TEntity>(TEntity item)
            where TEntity : BaseEntity, new();

        Task<int> DeleteAsync<TEntity>(TEntity item)
            where TEntity : BaseEntity, new();

        Task<List<TEntity>> GetAllAsync<TEntity>(bool isRecursive = false)
            where TEntity : BaseEntity, new();

        Task<List<TEntity>> GetAllAsync<TEntity>(Expression<Func<TEntity, bool>> expression)
            where TEntity : BaseEntity, new();

        Task<int> Add<TEntity>(IEnumerable<TEntity> items)
            where TEntity : BaseEntity, new();

        Task UpdateAsync<TEntity>(TEntity obj)
           where TEntity : BaseEntity, new();

        Task UpdateAllAsync<TEntity>(IList<TEntity> items)
            where TEntity : BaseEntity, new();
    }
}
