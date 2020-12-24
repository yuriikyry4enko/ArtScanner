using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArtScanner.Models.Analytics;
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
                _appFileSystemService.DeleteFile(_appFileSystemService.GetFilePath(item.ImageFileName, FileType.Image));
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

                lastParentId = itemsChildrens.FirstOrDefault()?.Id;

                foreach (var itmChild in itemsChildrens)
                {
                    if (_appFileSystemService.DoesImageExist(itmChild.ImageFileName))
                    {
                        _appFileSystemService.DeleteFile(_appFileSystemService.GetFilePath(itmChild.ImageFileName, FileType.Image));
                    }

                    if (_appFileSystemService.DoesAudioExist(itmChild.AudioFileName))
                    {
                        _appFileSystemService.DeleteFile(_appFileSystemService.GetFilePath(itmChild.AudioFileName, FileType.Audio));
                    }

                    await Connection.DeleteAsync<ItemEntity>(itmChild.LocalId);
                }


                if (!lastParentId.HasValue || lastParentId == -1)
                    IsLastParentId = true;
            }

            if (_appFileSystemService.DoesImageExist(item.ImageFileName))
            {
                _appFileSystemService.DeleteFile(_appFileSystemService.GetFilePath(item.ImageFileName, FileType.Image));
            }

            if (_appFileSystemService.DoesAudioExist(item.AudioFileName))
            {
                _appFileSystemService.DeleteFile(_appFileSystemService.GetFilePath(item.AudioFileName, FileType.Audio));
            }

            var itemEntity = await Connection.DeleteAsync<ItemEntity>(item.LocalId);

            return itemEntity;
        }


        public async Task<long> InsertOrUpdateWithChildren(ItemEntity item)
        {
            if (item.LocalId == 0)
            {
                item.ImageFileName = _appFileSystemService.SaveImage(item.ImageByteArray, $"{item.Title.Replace(" ", string.Empty)}{item.Id}.jpg");

                await Connection.InsertAsync(item);

            }
            else
            {
                await Connection.UpdateAsync(item);
            }

            return item.LocalId;
        }

        public async Task<List<ItemEntity>> GetAllMainFolders()
        {
            var folderItemEntities = from item in Connection.Table<ItemEntity>()
                                     where item.IsFolder && item.ParentId == -1
                                     select item;

            return await folderItemEntities.ToListAsync();
        }

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
        
        public async Task<List<ItemEntity>> GetItemsByPageWithChildren(bool isFolder, long parentId, int from = 0, int to = 10)
        {
            return await Connection.QueryAsync<ItemEntity>($"SELECT * FROM [ItemEntity] WHERE [ParentId]={parentId} LIMIT {from}, {to} ");
        }

        public async Task SaveAudioFileFromStream(ItemEntity item)
        {
            try
            {
                using (var bench = new Benchmark($"Save audio stream to file with id {item.Id} and lang {item.LangTag}"))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await item.AudioStream.CopyToAsync(memoryStream);
                        item.AudioByteArray = memoryStream.ToArray();
                    }

                    if (item.AudioByteArray != null)
                    {
                        item.AudioFileName = _appFileSystemService.SaveAudio(item.AudioByteArray, $"{item.Title.Replace(" ", string.Empty)}{item.Id}.mp3");
                        await Connection.UpdateAsync(item);
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Log(ex);
            }
        }

        //Stream GetQueueStream(ItemEntity item)
        //{
        //    var queueStream = new QueueStream(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + $"{item.Title.Replace(" ", string.Empty)}{ item.Id}.mp3");
        //    var t = new Thread((x) => {
        //        var tbuf = new byte[8192];
        //        int count;

        //        while ((count = item.AudioStream.Read(tbuf, 0, tbuf.Length)) != 0)
        //            queueStream.Push(tbuf, 0, count);

        //    });
        //    t.Start();
        //    return queueStream;
        //}
    }
}
