using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopBridgeBackEnd.DTO;
using ShopBridgeBackEnd.Entities;
using ShopBridgeBackEnd.Interface;
using ShopBridgeBackEnd.Helper;
using System.Text.Json;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShopBridgeBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepository inventoryRepository;
        private readonly IMapper mapper;

        public InventoryController(IInventoryRepository inventoryRepository, IMapper mapper)
        {
            this.inventoryRepository = inventoryRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<InventoryDTO>> CreateInventory(CreateInventory createInventory)
        {
            if (await inventoryRepository.GetInventoryByNameAsync(createInventory.Name) != null)
                return BadRequest("Inventory Name is already exist.");

            var inventoryToInsert = mapper.Map<Inventory>(createInventory);
            await inventoryRepository.InsertInventory(inventoryToInsert);

            return Ok(new InventoryDTO
            {
                Name = inventoryToInsert.Name,
                Description = inventoryToInsert.Description,
                Price = inventoryToInsert.Price
            });

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryDTO>>> GetInventories([FromQuery] InventoryParams inventoryParams)
        {
            var inventory = await inventoryRepository.GetInventoriesAsync(inventoryParams);

            var paginationHeader = new PaginationHeader(inventory.CurrentPage, inventory.PageSize,
                inventory.TotalCount, inventory.TotalPages);
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            Response.Headers.Add("Pagination", System.Text.Json.JsonSerializer.Serialize(paginationHeader));
            Response.Headers.Add("Access-Control-Expose-Headers", "Pagination");

            return Ok(inventory);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateInventory(InventoryUpdateDTO inventoryUpdateDTO)
        {

            var orgInventory = await inventoryRepository.GetInventoryByIDAsync(inventoryUpdateDTO.ID);
            mapper.Map(inventoryUpdateDTO, orgInventory);

            var result = await inventoryRepository.UpdateInventory(orgInventory);

            if (result) return Ok("Record updated successfully");

            return BadRequest("Fail to update user.");
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteInventory(InventoryUpdateDTO inventoryUpdateDTO)
        {
            var orgInventory = await inventoryRepository.GetInventoryByIDAsync(inventoryUpdateDTO.ID);

            var result = await inventoryRepository.DeleteInventory(orgInventory);

            if (result) return Ok($"Deleted Product:{inventoryUpdateDTO.Name}");

            return BadRequest("Fail to delete user.");
        }
    }
}
