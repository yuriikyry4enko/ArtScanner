using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ArtScanner.Models.Entities;
using ArtScanner.ViewModels;

namespace ArtScanner.Services
{
    class ItemDBService : BaseDBService, IItemDBService
    {
        private IAppFileSystemService _appFileSystemService;
        private IAppSettings _appSettings;

        public ItemDBService(
           IAppSettings appSettings,
           IAppFileSystemService appFileSystemService,
           IAppDatabase appDatabase) : base(appDatabase)
        {
            this._appFileSystemService = appFileSystemService;
            this._appSettings = appSettings;
        }

        public async Task<ItemEntity> GetByServerId(long id)
        {
            var item = await FindItem<ItemEntity>(x => x.Id == id);

            return item;
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

        public async Task<long> DeleateItemWithChildren(ItemEntity item)
        {
            var IsLastParentId = false;
            long? lastParentId = item.Id;

            while(!IsLastParentId)
            {
                var itemsChildrens = (await Connection.Table<ItemEntity>().ToListAsync()).Where(x => x.ParentId == lastParentId).ToList();

                //var itemsChildrens = await (from itemChildren in Connection.Table<ItemEntity>()
                //                            where itemChildren.ParentId == lastParentId
                //                            select itemChildren).ToListAsync();

                lastParentId = itemsChildrens.FirstOrDefault()?.Id;


                foreach (var itmChild in itemsChildrens)
                {
                    if (_appFileSystemService.DoesImageExist(itmChild.ImageFileName))
                    {
                        _appFileSystemService.DeleteFile(_appFileSystemService.GetFilePath(itmChild.ImageFileName));
                    }
                    await Connection.DeleteAsync<ItemEntity>(itmChild.LocalId);
                }


                if (!lastParentId.HasValue || lastParentId == -1)
                    IsLastParentId = true;
            }

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
                item.ImageFileName = item.Title + item.Id + ".jpg";

                _appFileSystemService.SaveImage(item.ImageByteArray, item.ImageFileName);

                await Connection.InsertAsync(item);
            }
            else
            {
                await Connection.UpdateAsync(item);
            }

            return item.LocalId;
        }

        //public async Task<Tuple<long, bool>> InsertOrUpdateCategoryWithChildren(CategoryItemEntity item, bool update = false)
        //{
            
        //    var categoryItemEntity = await FindItem<CategoryItemEntity>(x => x.Id == item.Id);

        //    if (item.LocalId == 0 && categoryItemEntity == null)
        //    {
        //        item.ImageFileName = item.Title + item.Id + ".jpg";

        //        _appFileSystemService.SaveImage(item.ImageByteArray, item.ImageFileName);

        //        await Connection.InsertAsync(item);
        //        return new Tuple<long, bool>(item.LocalId, true);
        //    }
        //    else if(update)
        //    {
        //        await Connection.UpdateAsync(item);
        //        return new Tuple<long, bool>(item.LocalId, false);
        //    }

        //    return null;
        //}

        //public async Task<Tuple<long, bool>> InsertOrUpdateFolderWithChildren(FolderItemEntity item, bool update = false)
        //{
        //    HomePageViewModel.NeedsToUpdate = true;

        //    var foundedItemInLocalDB = await FindItem<FolderItemEntity>(x => x.Id == item.Id);

        //    if (item.LocalId == 0 && foundedItemInLocalDB == null)
        //    {
        //        item.ImageFileName = item.Title + item.Id + ".jpg";

        //        _appFileSystemService.SaveImage(item.ImageByteArray, item.ImageFileName);

        //        await Connection.InsertAsync(item);
        //        return new Tuple<long, bool>(item.LocalId, true);
        //    }
        //    else if (update)
        //    {
        //        await Connection.UpdateAsync(item);
        //        return new Tuple<long, bool>(item.LocalId, false);
        //    }

        //    return null;
        //}

        public async Task<List<ItemEntity>> GetAllMainFolders()
        {
            var folderItemEntities = from item in Connection.Table<ItemEntity>()
                                     where item.IsFolder && item.ParentId == -1
                                     select item;

            return await folderItemEntities.ToListAsync();
        }

        //public async Task<List<CategoryItemEntity>> GetAllCategories()
        //{
        //    var categoryItemEntities = await Connection.Table<CategoryItemEntity>().ToListAsync();

        //    return categoryItemEntities;
        //}

        //public async Task<FolderItemEntity> FindFolderById(long id)
        //{
        //    var item = await FindItem<FolderItemEntity>(x => x.Id == id);

        //    return item;
        //}

        //public async Task<CategoryItemEntity> FindCategoryById(long id)
        //{
        //    var item = await FindItem<CategoryItemEntity>(x => x.Id == id);

        //    return item;
        //}

        public async Task<ItemEntity> FindItemEntityById(long id)
        {
            var item = await FindItem<ItemEntity>(x => x.Id == id);

            return item;
        }

        public async Task<List<ItemEntity>> GetItemsByParentIdAll(long parentId)
        {
            var items = from item in Connection.Table<ItemEntity>()
                                     where item.ParentId == parentId
                                     select item;


            return await items.ToListAsync();
        }

        public async Task<List<ItemEntity>> GetItemsByParentIdAllWithChildrens(long parentId)
        {
            var items = from item in Connection.Table<ItemEntity>()
                        where item.ParentId == parentId
                        select item;


            return await items.ToListAsync();
        }





        //public async Task<List<CategoryItemEntity>> GetCategoriesByParentIdAll(long parentId)
        //{
        //    var items = await GetAllCategories();

        //    return items.Where(x => x.ParentId == parentId).ToList();
        //}

        //public async Task DeleateFolderItem(FolderItemEntity item)
        //{
        //    try
        //    {
        //        var categoriesByFolderId = (await Connection.Table<CategoryItemEntity>().ToListAsync()).Where(x => x.ParentId == item.Id);

        //        foreach (var itemCategory in categoriesByFolderId)
        //        {
        //            await DeleateCategoryItem(itemCategory);
        //        }

        //        await Connection.DeleteAsync(item);
        //    }
        //    catch(Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    }
        //}

        //public async Task DeleateCategoryItem(CategoryItemEntity item)
        //{
        //    try
        //    {
        //        var itemsByCategoryId = (await Connection.Table<ItemEntity>().ToListAsync()).Where(x => x.ParentId == item.Id);

        //        foreach (var itemEntity in itemsByCategoryId)
        //        {
        //            await Connection.DeleteAsync(itemEntity);
        //        }

        //        await Connection.DeleteAsync(item);
        //    }
        //    catch(Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    }
        //}

        //public async Task CheckAndDeleteFolder(long parentFolderId, long localId)
        //{
        //    try
        //    {
        //        var categoriesByFolderId = (await Connection.Table<CategoryItemEntity>().ToListAsync()).Where(x => x.ParentId == parentFolderId);
        //        if(categoriesByFolderId.Count() == 0)
        //        {
        //            var currentFolder = await FindItem<FolderItemEntity>(x => x.LocalId == localId);
        //            await Connection.DeleteAsync<FolderItemEntity>(currentFolder);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    }
        //}
    }
}
