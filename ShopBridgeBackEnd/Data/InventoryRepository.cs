using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopBridgeBackEnd.Entities;
using ShopBridgeBackEnd.Helper;
using ShopBridgeBackEnd.Interface;
using ShopBridgeBackEnd.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShopBridgeBackEnd.DTO;

namespace ShopBridgeBackEnd.Data
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public InventoryRepository(DataContext dataContext,IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        public async Task<Inventory> GetInventoryByNameAsync(string inventoryName)
        {
          return await dataContext.Inventories
                .Where(inv => inv.Name.ToLower() == inventoryName.ToLower())
                //.ProjectTo<Inventory>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> InsertInventory(Inventory inventory)
        {
            dataContext.Inventories.Add(inventory);
            if (await SaveAllAsync())
                return true;
            return false;
        }

        public async Task<bool> UpdateInventory(Inventory inventory)
        {
            dataContext.Entry(inventory).State = EntityState.Modified;
            if (await SaveAllAsync())
                return true;
            return false;
        }

        public async Task<bool> DeleteInventory(Inventory inventory)
        {
            dataContext.Inventories.Remove(inventory);
            if (await SaveAllAsync())
                return true;
            return false;
        }

        public async Task<PagedList<InventoryDTO>> GetInventoriesAsync(InventoryParams inventoryParams)
        {
            var query = dataContext.Inventories.AsQueryable();

            return await PagedList<InventoryDTO>.CreateAsync(query.ProjectTo<InventoryDTO>
                (mapper.ConfigurationProvider).AsNoTracking(),
                inventoryParams.PageNumber, inventoryParams.PageSize);
        }

        public async Task<Inventory> GetInventoryByIDAsync(int inventoryID)
        {
            return await dataContext.Inventories
                 .Where(inv => inv.Id == inventoryID)
                 //.ProjectTo<Inventory>(mapper.ConfigurationProvider)
                 .SingleOrDefaultAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await dataContext.SaveChangesAsync() > 0;
        }

    }
}
