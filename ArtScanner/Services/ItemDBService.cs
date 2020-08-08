using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArtScanner.Models.Entities;

namespace ArtScanner.Services
{
    class ItemDBService : BaseDBService, IItemDBService
    {   
        public ItemDBService(
           IAppDatabase appDatabase) : base(appDatabase)
        {

        }

        public async Task<List<ItemEntity>> GetAll()
        {
            var itemEntities = await Connection.Table<ItemEntity>().ToListAsync();

            return itemEntities;
        }

        public async Task<ItemEntity> GetByIdWithChildren(long id)
        {
            var itemEntity = await Connection.FindAsync<ItemEntity>(id);

            return itemEntity;
        }

        public async Task<long> InsertOrUpdateWithChildren(ItemEntity item)
        {
            if (item.LocalId == 0)
            {
                await Connection.InsertAsync(item);
            }
            else
            {
                await Connection.UpdateAsync(item);
            }

            return item.LocalId;
        }
    }
}
