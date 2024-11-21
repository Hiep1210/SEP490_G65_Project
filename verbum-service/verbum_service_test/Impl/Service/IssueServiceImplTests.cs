using Microsoft.EntityFrameworkCore;
using verbum_service_domain.Models;
using verbum_service_infrastructure.DataContext;

namespace verbum_service_test.Impl.Service
{
    public class IssueServiceImplTests
    {
        private async Task<verbumContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<verbumContext>()
                .UseInMemoryDatabase(databaseName: "verbum2").Options;
            var dbContext = new verbumContext(options);
            dbContext.Database.EnsureCreated();

            return dbContext;
        }

        public async Task GetAllIssue()
        {

        }
    }
}
