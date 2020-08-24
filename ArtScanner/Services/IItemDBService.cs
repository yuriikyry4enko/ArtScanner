using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArtScanner.Models.Entities;

namespace ArtScanner.Services
{
    public interface IItemDBService
    {
        Task<long> InsertOrUpdateWithChildren(ItemEntity item);

        Task<ItemEntity> GetByIdWithChildren(long id);

        Task<long> DeleateItem(ItemEntity item);

        Task<List<ItemEntity>> GetAll();

        Task<ItemEntity> GetByServerId(string id);
    }
}
