using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArtScanner.Models.Entities;

namespace ArtScanner.Services
{
    public interface IItemDBService
    {
        Task<long> InsertOrUpdateWithChildren(ItemEntity item);

        Task<long> DeleateItemWithChildren(ItemEntity item);

        Task<ItemEntity> GetByIdWithChildren(long id);

        Task<long> DeleateItem(ItemEntity item);

        Task<List<ItemEntity>> GetAll();

        Task<List<ItemEntity>> GetItemsByParentIdAll(long parentId);

        Task<List<ItemEntity>> GetAllMainFolders();

        Task<ItemEntity> GetByServerId(long id);

        Task<ItemEntity> FindItemEntityById(long id);

        Task<List<ItemEntity>> GetItemsByPageWithChildren(bool isFolder, long parentId, int from = 0, int to = 10);

        Task SaveAudioFileFromStream(ItemEntity item);
    }
}
