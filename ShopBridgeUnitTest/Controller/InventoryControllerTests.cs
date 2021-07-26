using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopBridgeBackEnd;
using ShopBridgeBackEnd.Controllers;
using ShopBridgeBackEnd.DTO;
using ShopBridgeBackEnd.Entities;
using ShopBridgeBackEnd.Helper;
using ShopBridgeBackEnd.Interface;
using Xunit;

namespace ShopBridgeUnitTest.Controller
{
    public class InventoryControllerTests
    {
        private readonly static Mock<IInventoryRepository> iInventoryRepository = new Mock<IInventoryRepository>();

        [Fact]
        public void InventoryControllerConstructorTest()
        {
            InventoryController();
        }


        [Fact]
        public void CreateInventoryProductExistTest()
        {
            iInventoryRepository.Setup(c => c.GetInventoryByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(getInventory()));

            InventoryController inventoryController = InventoryController();

            var result =  inventoryController.CreateInventory(createInventory());

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result.Result.Result);
            
        }

        [Fact]
        public void CreateInventorySuccessTest()
        {
            CreateInventory createInventory = new CreateInventory()
            {
                Name = "TestProd2",
                Price = 2020,
                Description = "TestDesc2"
            };
            Inventory inventory = null;

            iInventoryRepository.Setup(c => c.GetInventoryByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(inventory));
            iInventoryRepository.Setup(c => c.InsertInventory(It.IsAny<Inventory>())).Returns(Task.FromResult(true));

            InventoryController inventoryController = InventoryController();

            var result = inventoryController.CreateInventory(createInventory);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result.Result);

        }

        [Fact]
        public void GetInventoriesTest()
        {
            InventoryParams inventoryParams = new InventoryParams()
            {
                PageNumber=1,
                PageSize=1
            };
            InventoryDTO inventoryDTO = new InventoryDTO()
            {
                Name = "TestProd2",
                Price = 2020,
                Description = "TestDesc2"

            };

            var inventoryDTOList = new List<InventoryDTO>();
            inventoryDTOList.Add(inventoryDTO);
            inventoryDTOList.Add(inventoryDTO);

            var queryable = inventoryDTOList.AsQueryable();

            PagedList<InventoryDTO> inventoryDTOs = new PagedList<InventoryDTO>(queryable, 2,1,1);

            iInventoryRepository.Setup(c => c.GetInventoriesAsync(inventoryParams)).Returns(Task.FromResult(inventoryDTOs));

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

        [Fact]
        public void GetInventoriesExceptionTest()
        {
            InventoryParams inventoryParams = new InventoryParams()
            {
                PageNumber = 1,
                PageSize = 1
            };
            InventoryDTO inventoryDTO = new InventoryDTO()
            {
                Name = "",
                Price = 2020,
                Description = ""

            };

            var inventoryDTOList = new List<InventoryDTO>();
            inventoryDTOList.Add(inventoryDTO);
            inventoryDTOList.Add(inventoryDTO);

            var queryable = inventoryDTOList.AsQueryable();

            PagedList<InventoryDTO> inventoryDTOs = new PagedList<InventoryDTO>(queryable, 2, 1, 1);

            iInventoryRepository.Setup(c => c.GetInventoriesAsync(inventoryParams)).ThrowsAsync(new Exception("Test exception"));
            //.Returns(Task.FromResult(inventoryDTOs)).Throws();

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

            Assert.NotNull(result.Exception);
        }

        [Fact]
        public void UpdateInventorySuccessTest()
        {
            InventoryUpdateDTO inventoryUpdateDTO = new InventoryUpdateDTO()
            {
                Name = "TestProd2",
                Price = 2020,
                Description = "TestDesc2",
                ID=2
            };

            iInventoryRepository.Setup(c => c.GetInventoryByIDAsync(It.IsAny<int>())).Returns(Task.FromResult(getInventory()));
            iInventoryRepository.Setup(c => c.UpdateInventory(It.IsAny<Inventory>())).Returns(Task.FromResult(true));

            InventoryController inventoryController = InventoryController();

            var result = inventoryController.UpdateInventory(inventoryUpdateDTO);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);

        }

        [Fact]
        public void UpdateInventoryFailureTest()
        {
            InventoryUpdateDTO inventoryUpdateDTO = new InventoryUpdateDTO()
            {
                Name = "TestProd2",
                Price = 2020,
                Description = "TestDesc2",
                ID = 2
            };

            iInventoryRepository.Setup(c => c.GetInventoryByIDAsync(It.IsAny<int>())).Returns(Task.FromResult(getInventory()));
            iInventoryRepository.Setup(c => c.UpdateInventory(It.IsAny<Inventory>())).Returns(Task.FromResult(false));

            InventoryController inventoryController = InventoryController();

            var result = inventoryController.UpdateInventory(inventoryUpdateDTO);

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result.Result);

        }

        [Fact]
        public void DeleteInventorySuccessTest()
        {
            InventoryUpdateDTO inventoryUpdateDTO = new InventoryUpdateDTO()
            {
                Name = "TestProd2",
                Price = 2020,
                Description = "TestDesc2",
                ID = 2
            };

            iInventoryRepository.Setup(c => c.GetInventoryByIDAsync(It.IsAny<int>())).Returns(Task.FromResult(getInventory()));
            iInventoryRepository.Setup(c => c.DeleteInventory(It.IsAny<Inventory>())).Returns(Task.FromResult(true));

            InventoryController inventoryController = InventoryController();

            var result = inventoryController.DeleteInventory(inventoryUpdateDTO);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);

        }

        [Fact]
        public void DeleteInventoryFailureTest()
        {
            InventoryUpdateDTO inventoryUpdateDTO = new InventoryUpdateDTO()
            {
                Name = "TestProd2",
                Price = 2020,
                Description = "TestDesc2",
                ID = 2
            };

            iInventoryRepository.Setup(c => c.GetInventoryByIDAsync(It.IsAny<int>())).Returns(Task.FromResult(getInventory()));
            iInventoryRepository.Setup(c => c.DeleteInventory(It.IsAny<Inventory>())).Returns(Task.FromResult(false));

            InventoryController inventoryController = InventoryController();

            var result = inventoryController.DeleteInventory(inventoryUpdateDTO);

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result.Result);
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

            return new InventoryController(iInventoryRepository.Object, mapper);
        }

        private CreateInventory createInventory()
        {
            return new CreateInventory()
            {
                Name = "TestProd1",
                Price = 2020,
                Description = "TestDesc1"
            };
        }


        private Inventory getInventory()
        {
            return new Inventory()
            {
                Name = "TestProd1",
                Price = 2020,
                Description = "TestDesc1"
            };
        }
    }

}
