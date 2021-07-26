using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ShopBridgeBackEnd.DTO;
using ShopBridgeBackEnd.Entities;

namespace ShopBridgeBackEnd
{
    public class AutoMapperProfile :Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateInventory,Inventory>();
            CreateMap<Inventory, InventoryDTO>();
            CreateMap<InventoryDTO,Inventory>();
            CreateMap<InventoryUpdateDTO, Inventory>();
        }
    }
}
