using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ShopBridgeBackEnd;
using ShopBridgeBackEnd.Data;
using ShopBridgeBackEnd.DTO;
using ShopBridgeBackEnd.Entities;
using ShopBridgeBackEnd.Interface;
using ShopBridgeBackEnd.Controllers;
using Moq;
using AutoMapper;
using ShopBridgeBackEnd.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShopBridgeUnitTest.TestController
{
    public class TestInventoryController
    {
        private static Mock<IInventoryRepository> inventoryRepo = new Mock<IInventoryRepository>();
        
        [Fact]
        public void GetInventories_ValidInventoryParams_ReturnInventoryDTO()
        {
            InventoryParams inventoryParams = new InventoryParams()
            {
                PageNumber = 1,
                PageSize = 1
            };

            InventoryDTO inventoryDTO = new InventoryDTO()
            {
                Name = "test1",
                Description = "test1 description",
                Price = 1899
            };

            var inventoryDTOList = new List<InventoryDTO>();
            inventoryDTOList.Add(inventoryDTO);
            inventoryDTOList.Add(inventoryDTO);

            var queryable = inventoryDTOList.AsQueryable();

            PagedList<InventoryDTO> inventoryDTOs = new PagedList<InventoryDTO>(queryable, 2, 1, 1);

            inventoryRepo.Setup(c => c.GetInventoriesAsync(inventoryParams)).Returns(Task.FromResult(inventoryDTOs));

            InventoryController inventoryController = InventoryController();
            var headerDictionary = new HeaderDictionary();
            var response = new Mock<HttpResponse>();
            response.SetupGet(r => r.Headers).Returns(headerDictionary);

            var httpContext = new Mock<HttpContext>();
            httpContext.SetupGet(a => a.Response).Returns(response.Object);

            inventoryController.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext.Object
            };

            var result = inventoryController.GetInventories(inventoryParams);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result.Result);
        }


        private static InventoryController InventoryController()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
                cfg.CreateMap<CreateInventory, Inventory>();
                cfg.CreateMap<Inventory, InventoryDTO>();
                cfg.CreateMap<InventoryDTO, Inventory>();
                cfg.CreateMap<InventoryUpdateDTO, Inventory>();
            });
            var mapper = mockMapper.CreateMapper();

            return new InventoryController(inventoryRepo.Object, mapper);
        }
    }
}
