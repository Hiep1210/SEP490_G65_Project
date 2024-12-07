using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using verbum_service_domain.DTO.Request;
using verbum_service_infrastructure.DataContext;
using verbum_service_infrastructure.Impl.Validation;

namespace verbum_service_test.Impl.Validation
{
    [TestClass]
    public class DeleteCategoryValidationTest
    {
        private async Task<verbumContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<verbumContext>()
                .UseInMemoryDatabase(databaseName: "verbum2").Options;
            var dbContext = new verbumContext(options);
            dbContext.Database.EnsureCreated();

            return dbContext;
        }

        [TestMethod]
        public async Task DeleteCategory_EmptyId()
        {
            var dbContext = await GetDatabaseContext();
            var validation = new DeleteCategoryValidation(dbContext);

            CategoryDelete categoryDelete = new CategoryDelete
            {
                Id = 0
            };

            //Act
            List<string> result = await validation.Validate(categoryDelete);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("CategoryId is required"));
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task DeleteCategory_Exist()
        {
            var dbContext = await GetDatabaseContext();
            var validation = new DeleteCategoryValidation(dbContext);

            CategoryDelete categoryDelete = new CategoryDelete
            {
                Id = 77
            };

            //Act
            List<string> result = await validation.Validate(categoryDelete);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("Category is not found in database"));
            Assert.AreEqual(1, result.Count());
        }
    }
}
