using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ArtScanner.Models.Entities;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;

namespace ArtScanner.Services
{
    class BaseDBService : IBaseDBService
    {
        protected readonly IAppDatabase appDatabase;

        public SQLiteAsyncConnection Connection => appDatabase.Connection;

        public BaseDBService(
            IAppDatabase appDatabase)
        {
            this.appDatabase = appDatabase;
        }

        public Task<int> DeleteAllAsync<TEntity>(IEnumerable<TEntity> items)
           where TEntity : BaseEntity, new()
           => Connection.DeleteAllAsync<TEntity>();

        public Task<TEntity> GetAsync<TEntity>(long id)
            where TEntity : BaseEntity, new()
            => Connection.GetWithChildrenAsync<TEntity>(id, recursive: true);

        public async Task<TEntity> GetAsync<TEntity>(Expression<Func<TEntity, bool>> expression)
            where TEntity : BaseEntity, new()
        {
            var result = await Connection.GetAsync<TEntity>(expression);
            if (result != null)
                result = await GetAsync<TEntity>(result.LocalId);

            return result;
        }

        public Task<int> DeleteAsync<TEntity>(TEntity item)
            where TEntity : BaseEntity, new()
            => Connection.DeleteAsync(item);

        public Task<List<TEntity>> GetAllAsync<TEntity>(bool isRecursive = false)
            where TEntity : BaseEntity, new()
            => Connection.GetAllWithChildrenAsync<TEntity>(recursive: isRecursive);

        public Task<List<TEntity>> GetAllAsync<TEntity>(Expression<Func<TEntity, bool>> expression)
            where TEntity : BaseEntity, new()
            => Connection.GetAllWithChildrenAsync<TEntity>(expression);

        public Task Add<TEntity>(TEntity item)
            where TEntity : BaseEntity, new()
            => Connection.InsertWithChildrenAsync(item, true);

        public Task<int> Add<TEntity>(IEnumerable<TEntity> items)
            where TEntity : BaseEntity, new()
         => Connection.InsertAllAsync(items);

        public Task UpdateAsync<TEntity>(TEntity obj)
            where TEntity : BaseEntity, new()
         => Connection.UpdateAsync(obj);

        public Task UpdateAllAsync<TEntity>(IList<TEntity> items)
            where TEntity : BaseEntity, new()
            => Connection.UpdateAllAsync(items);
    }
}
