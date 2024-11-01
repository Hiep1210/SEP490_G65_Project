using Microsoft.VisualStudio.TestTools.UnitTesting;
using verbum_service_infrastructure.Impl.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using verbum_service_infrastructure.DataContext;
using verbum_service_application.Service;
using verbum_service_domain.DTO.Request;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using verbum_service_domain.Common;
using verbum_service_domain.Models;
using Microsoft.EntityFrameworkCore.Query;
using AutoMapper;
using Microsoft.AspNetCore.Localization;
using verbum_service_domain.Common.ErrorModel;

namespace verbum_service_test
{
    [TestClass]
    public class OrderServiceImplTests
    {
        private Mock<verbumContext> context;
        private Mock<DbSet<Order>> mockOrderSet;
        private OrderServiceImpl orderService;
        private Mock<IMapper> mapper;
        private Mock<CurrentUser> currentUser;
        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<verbumContext>()
                .UseInMemoryDatabase(databaseName: "verbum")
                .Options;
            context = new Mock<verbumContext>(options);
            mockOrderSet = new Mock<DbSet<Order>>();
            mapper = new Mock<IMapper>();
            currentUser = new Mock<CurrentUser>();
            context.Setup(c => c.Orders).Returns(mockOrderSet.Object);
            orderService = new OrderServiceImpl(context.Object, mapper.Object, currentUser.Object);
        }
        //[TestCleanup]
        //public void Cleanup()
        //{
        //    context.Database.EnsureDeleted();
        //    context.Dispose();
        //}

        [TestMethod]
        [ExpectedException(typeof(BusinessException))]
        public async Task AcceptOrDeclineOrder_ShouldThrowException_WhenStatusIsInvalid()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            string invalidStatus = "INVALID_STATUS";

            // Act
            await orderService.ChangeOrderStatus(orderId, invalidStatus);

            // Assert is handled by the ExpectedException attribute
        }

        [TestMethod]
        public async Task ShouldAcceptOrDeclineOrderSuccess()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            string validStatus = OrderStatus.ACCEPTED.ToString();

            //mockOrderSet.Setup(m => m.Where(It.IsAny<Expression<Func<Order, bool>>>()))
            //             .Returns(mockOrderSet.Object);

            //mockOrderSet.Setup(m => m.ExecuteUpdateAsync(It.IsAny<Expression<Func<SetPropertyCalls<Order>, SetPropertyCalls<Order>>>>(), default))
            //             .ReturnsAsync(1); // Simulate successful update

            // Act
            await orderService.ChangeOrderStatus(orderId, validStatus);

            // Assert
            mockOrderSet.Verify(m => m.ExecuteUpdateAsync(It.IsAny<Expression<Func<SetPropertyCalls<Order>, SetPropertyCalls<Order>>>>(), default), Times.Once);
        }
        //[TestMethod]
        //public async Task ShouldAcceptOrDeclineOrderSuccess()
        //{
        //    var orderId = Guid.NewGuid();
        //    var validStatus = OrderStatus.ACCEPTED.ToString();

        //    var dbSetMock = new Mock<DbSet<Order>>();
        //    context.Setup(c => c.Orders).Returns(dbSetMock.Object);
        //    dbSetMock.Setup(d => d.Where(It.IsAny<Expression<Func<Order, bool>>>()))
        //             .Returns(dbSetMock.Object);
        //    dbSetMock.Setup(d => d.ExecuteUpdateAsync(It.IsAny<Expression<Func<SetPropertyCalls<Order>, SetPropertyCalls<Order>>>>(), default))
        //             .ReturnsAsync(1);

        //    // Act
        //    await _orderService.AcceptOrDeclineOrder(orderId, validStatus);
        //}

        private List<UploadOrderFileRequest> buildUploadOrderFileRequest()
        {
            List<UploadOrderFileRequest> mockData = new List<UploadOrderFileRequest>
            {
                new UploadOrderFileRequest
                {
                    OrderId = Guid.NewGuid(),
                    ReferenceFileUrl = "link to file",
                    Tag = "TRANSLATION"
                }
            };
            return mockData;
        }
    }

}