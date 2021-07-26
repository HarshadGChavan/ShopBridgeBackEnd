using System.Collections.Generic;
using System.Threading.Tasks;
using ShopBridgeBackEnd.DTO;
using ShopBridgeBackEnd.Entities;
using ShopBridgeBackEnd.Helper;

namespace ShopBridgeBackEnd.Interface
{
    public interface IInventoryRepository
    {
        Task<PagedList<InventoryDTO>> GetInventoriesAsync(InventoryParams inventoryParams);

        Task<bool> InsertInventory(Inventory inventory);

        Task<bool> DeleteInventory(Inventory inventory);

        Task<bool> UpdateInventory(Inventory inventory);

        Task<Inventory> GetInventoryByNameAsync(string inventoryName);

        Task<Inventory> GetInventoryByIDAsync(int inventoryID);
        Task<bool> SaveAllAsync();
    }
}
