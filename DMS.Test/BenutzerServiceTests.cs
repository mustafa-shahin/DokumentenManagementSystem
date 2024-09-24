using DMS.DataAccess;
using DMS.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DMS.Tests
{
    public class BenutzerServiceTests
    {
        private DataContext GetCreateTestDbContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Ensure unique database per test run
                .Options;
            return new DataContext(options); // Use in-memory database for tests
        }

        [Fact]
        public async Task BenutzerService_ShouldCreateUser()
        {
            var context = GetCreateTestDbContext();
            var service = new BenutzerService(context);

            //string Username = "testuser_" + Guid.NewGuid().ToString();

            var result = await service.CreateUser("testuser", "testpassword");

            Assert.True(result, "User creation failed.");
            var createdUser = await context.Benutzer.FirstOrDefaultAsync(b => b.Name == "testuser");
            Assert.NotNull(createdUser);
            Assert.Equal("testuser", createdUser.Name);
            Assert.Equal("testpassword", createdUser.Passwort);
        }

        [Fact]
        public async Task BenutzerService_ShouldNotCreateDuplicateUser()
        {
            var context = GetCreateTestDbContext();
            var service = new BenutzerService(context);

            //string uniqueUsername = "testuser_" + Guid.NewGuid();
            await service.CreateUser("testuser", "testpassword");
            var result = await service.CreateUser("testuser", "anotherpassword");

            Assert.False(result, "Duplicate user creation should fail.");
        }

        [Fact]
        public async Task BenutzerService_ShouldLoginUser()
        {
            var context = GetCreateTestDbContext();
            var service = new BenutzerService(context);

            //string uniqueUsername = "testuser_" + Guid.NewGuid().ToString();
            await service.CreateUser("testuser", "testpassword");

            var user = await service.LoginUser("testuser", "testpassword");

            Assert.NotNull(user);
            Assert.Equal("testuser", user.Name);
        }

        [Fact]
        public async Task BenutzerService_ShouldNotLoginWithIncorrectPassword()
        {
            // Arrange
            var context = GetCreateTestDbContext();
            var service = new BenutzerService(context);

            //string uniqueUsername = "testuser_" + Guid.NewGuid().ToString();
            await service.CreateUser("testuser", "testpassword");

            var user = await service.LoginUser("testuser", "wrongpassword");

            Assert.Null(user);
        }
    }
}
