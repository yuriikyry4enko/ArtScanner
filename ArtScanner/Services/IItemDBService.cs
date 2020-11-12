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

        //Task<Tuple<long, bool>> InsertOrUpdateCategoryWithChildren(CategoryItemEntity item, bool update = false);

        //Task<Tuple<long, bool>> InsertOrUpdateFolderWithChildren(FolderItemEntity item, bool update = false);

        Task<ItemEntity> GetByIdWithChildren(long id);

        Task<long> DeleateItem(ItemEntity item);

        //Task DeleateFolderItem(FolderItemEntity item);

        //Task DeleateCategoryItem(CategoryItemEntity item);

        //Task CheckAndDeleteFolder(long parentFolderId, long localId);

        Task<List<ItemEntity>> GetAll();

        Task<List<ItemEntity>> GetItemsByParentIdAll(long parentId);

        //Task<List<CategoryItemEntity>> GetCategoriesByParentIdAll(long parentId);

        Task<List<ItemEntity>> GetAllMainFolders();

        //Task<List<CategoryItemEntity>> GetAllCategories();

        Task<ItemEntity> GetByServerId(long id);

        //Task<FolderItemEntity> FindFolderById(long id);

        //Task<CategoryItemEntity> FindCategoryById(long id);

        Task<ItemEntity> FindItemEntityById(long id);
    }
}
