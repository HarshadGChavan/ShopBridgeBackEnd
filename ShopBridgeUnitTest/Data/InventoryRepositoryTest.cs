using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopBridgeBackEnd;
using ShopBridgeBackEnd.Data;
using ShopBridgeBackEnd.DTO;
using ShopBridgeBackEnd.Entities;
using ShopBridgeBackEnd.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShopBridgeUnitTest.Data
{
    
    public class InventoryRepositoryTest
    {
        [Fact]
        public void GetInventoryByNameTest()
        {

            var inventoryRepository = InventoryRepository();
            var result = inventoryRepository.GetInventoryByNameAsync("TestProd1").Result;
            Assert.NotNull(result);
            Assert.Equal("TestProd1", result.Name);
        }

        [Fact]
        public void InsertInventoryTest()
        {
            var inventoryRepository = InventoryRepository();
            Inventory inventory = new Inventory() { Id = 3, Name = "TestProd3", Price = 2500, Description = "TestDesc3" };

            var result = inventoryRepository.InsertInventory(inventory).Result;
            Assert.True(result);
        }

        [Fact]
        public void UpdateInventoryTest()
        {
            Inventory inventory = new Inventory() { Id = 4, Name = "TestProd4", Price = 2560, Description = "TestDesc4" };

            var options = new DbContextOptionsBuilder<DataContext>()
           .UseInMemoryDatabase(databaseName: "TestDb")
           .Options;

            using (var context = new DataContext(options))
            {
                context.Inventories.Add(new Inventory { Id = 4, Name = "TestProd4", Price = 2560, Description = "TestDesc4" });
                context.SaveChanges();
            }

            using (var context = new DataContext(options))
            {
                IMapper mapper = MockMapper();

                var inventoryRepository = new InventoryRepository(context, mapper);

                var result = inventoryRepository.UpdateInventory(inventory).Result;

                Assert.True(result);

            }
        }

        [Fact]
        public void DeleteInventoryTest()
        {
            Inventory inventory = new Inventory() { Id = 4, Name = "TestProd4", Price = 2560, Description = "TestDesc4" };

            var options = new DbContextOptionsBuilder<DataContext>()
           .UseInMemoryDatabase(databaseName: "TestDeleteDb")
           .Options;

            using (var context = new DataContext(options))
            {
                context.Inventories.Add(new Inventory { Id = 4, Name = "TestProd4", Price = 2560, Description = "TestDesc4" });
                context.SaveChanges();
            }

            using (var context = new DataContext(options))
            {
                IMapper mapper = MockMapper();

                var inventoryRepository = new InventoryRepository(context, mapper);

                var result = inventoryRepository.DeleteInventory(inventory).Result;

                Assert.True(result);

            }
        }


        public static DataContext GetDataContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

            var context = new DataContext(options);
            
            var s1= new ShopBridgeBackEnd.Entities.Inventory { Id = 1, Name = "TestProd1", Price = 2020, Description = "TestDesc1" };
            var s2= new ShopBridgeBackEnd.Entities.Inventory { Id = 2, Name = "TestProd2", Price = 2000, Description = "TestDesc2" };

            context.Inventories.Add(s1);
            context.Inventories.Add(s2);

            context.SaveChanges();

            return context;
        }

        private static InventoryRepository InventoryRepository()
        {
            IMapper mapper = MockMapper();

            return new InventoryRepository(GetDataContext(), mapper);
        }

        private static IMapper MockMapper()
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
            return mapper;
        }
    }
}
