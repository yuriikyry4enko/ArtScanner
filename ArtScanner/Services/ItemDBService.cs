using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArtScanner.Models.Entities;

namespace ArtScanner.Services
{
    class ItemDBService : BaseDBService, IItemDBService
    {
        private IAppFileSystemService _appFileSystemService;
        public ItemDBService(
           IAppFileSystemService appFileSystemService,
           IAppDatabase appDatabase) : base(appDatabase)
        {
            this._appFileSystemService = appFileSystemService;

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

        public async Task<long> DeleateItem(ItemEntity item)
        {

            if (_appFileSystemService.DoesImageExist(item.ImageFileName))
            {
                _appFileSystemService.DeleteFile(_appFileSystemService.GetFilePath(item.ImageFileName));
            }

            var itemEntity = await Connection.DeleteAsync<ItemEntity>(item.LocalId);

            return itemEntity;
        }

        public async Task<long> InsertOrUpdateWithChildren(ItemEntity item)
        {
            if (item.LocalId == 0)
            {
                item.ImageFileName = item.Id + ".jpg";

                _appFileSystemService.SaveImage(item.ImageByteArray, item.ImageFileName);

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
