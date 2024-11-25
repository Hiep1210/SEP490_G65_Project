using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using verbum_service_infrastructure.DataContext;
using verbum_service_domain.Models;
using AutoMapper;
using Moq;
using verbum_service_domain.Common;
using verbum_service_domain.DTO.Response;
using verbum_service_infrastructure.Impl.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace verbum_service_test.Impl.Service
{
    [TestClass]
    public class WorkServiceImplTests
    {
        private async Task<verbumContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<verbumContext>()
                .UseInMemoryDatabase(databaseName: "verbum2").Options;
            var dbContext = new verbumContext(options);
            dbContext.Database.EnsureCreated();

            if(await dbContext.Works.CountAsync() <= 0)
            {
                dbContext.Works.Add(new Work
                {
                    WorkId = Guid.Parse("7ed92254-895a-4644-a96f-fe8d3ab3ae70"),
                    OrderId = Guid.Parse("e5a521cc-ec2d-4034-bf83-68035577bed5"),
                    ServiceCode = "TL"
                });

                dbContext.Works.Add(new Work
                {
                    WorkId = Guid.Parse("d4ea0069-55f4-4850-a693-490033c3f692"),
                    OrderId = Guid.Parse("e5a521cc-ec2d-4034-bf83-68035577bed5"),
                    ServiceCode = "ED"
                });

                dbContext.Works.Add(new Work
                {
                    WorkId = Guid.Parse("d4ea0069-55f4-4850-a693-490033c3f692"),
                    OrderId = Guid.Parse("e5a521cc-ec2d-4034-bf83-68035577bed5"),
                    ServiceCode = "EV"
                });
            }

            return dbContext;
        }

        public async Task GetWork_TranslateManager()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            var mockMapper = new Mock<IMapper>();

            var currentUser = new CurrentUser
            {
                Id = Guid.Parse("80d4d6dd-8f0a-479e-b4a6-1016f34ec78a"),
                Email = "test@example.com",
                Name = "Test User",
                Status = "Active",
                Role = "CLIENT"
            };

            var mockIConfiguration = new Mock<IConfiguration>();
            var mockIhttpcontextAccessor = new Mock<IHttpContextAccessor>();

            var orderService = new OrderServiceImpl(dbContext, mockMapper.Object, currentUser, mockIConfiguration.Object, mockIhttpcontextAccessor.Object);

            mockMapper.Setup(m => m.Map<IEnumerable<OrderDetailsResponse>>(It.IsAny<IEnumerable<Order>>()))
                      .Returns(new List<OrderDetailsResponse>
                      {
                          new OrderDetailsResponse{ OrderId = Guid.NewGuid() },
                          new OrderDetailsResponse { OrderId = Guid.NewGuid() },
                      });

            //Act
            var result = orderService.GetAllOrder();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Result.Count());
        }
    }
}
