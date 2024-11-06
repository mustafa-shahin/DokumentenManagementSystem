using DMS.DataAccess;
using DMS.Service;
using Microsoft.EntityFrameworkCore;

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

            await service.CreateUser("testuser", "testpassword");
            var result = await service.CreateUser("testuser", "anotherpassword");

            Assert.False(result, "Duplicate user creation should fail.");
        }

        [Fact]
        public async Task BenutzerService_ShouldLoginUser()
        {
            var context = GetCreateTestDbContext();
            var service = new BenutzerService(context);

            await service.CreateUser("testuser", "testpassword");

            var createdUser = await context.Benutzer.FirstOrDefaultAsync(b => b.Name == "testuser");
            createdUser.IsActive = true;
            await context.SaveChangesAsync();

            var user = await service.LoginUser("testuser", "testpassword");

            Assert.NotNull(user);
            Assert.Equal("testuser", user.Name);
        }

        [Fact]
        public async Task BenutzerService_ShouldNotLoginWithIncorrectPassword()
        {
            var context = GetCreateTestDbContext();
            var service = new BenutzerService(context);

            await service.CreateUser("testuser", "testpassword");

            var user = await service.LoginUser("testuser", "wrongpassword");

            Assert.Null(user);
        }

        [Fact]
        public async Task BenutzerService_ShouldChangePasswordSuccessfully()
        {
            var context = GetCreateTestDbContext();
            var service = new BenutzerService(context);

            await service.CreateUser("testuser", "oldpassword");

            var changeResult = await service.ChangePassword("testuser", "newpassword");
            Assert.True(changeResult, "Password change should succeed.");

            var updatedUser = await context.Benutzer.FirstOrDefaultAsync(b => b.Name == "testuser");
            Assert.NotNull(updatedUser);
            Assert.Equal("newpassword", updatedUser.Passwort);
        }

        [Fact]
        public async Task BenutzerService_ShouldNotChangePasswordForNonExistingUser()
        {
            var context = GetCreateTestDbContext();
            var service = new BenutzerService(context);

            var result = await service.ChangePassword("nonexistentuser", "newpassword");

            Assert.False(result, "Password change should fail for non-existing user.");
        }

        [Fact]
        public async Task BenutzerService_ShouldGetAllUsers()
        {
            var context = GetCreateTestDbContext();
            var service = new BenutzerService(context);

            await service.CreateUser("user1", "password1");
            await service.CreateUser("user2", "password2");

            var users = await service.GetAllUsers();

            Assert.Equal(2, users.Count);
            Assert.Contains(users, u => u.Name == "user1");
            Assert.Contains(users, u => u.Name == "user2");
        }

        [Fact]
        public async Task BenutzerService_ShouldSaveChangesToUser()
        {
            var context = GetCreateTestDbContext();
            var service = new BenutzerService(context);

            await service.CreateUser("testuser", "testpassword");

            var createdUser = await context.Benutzer.FirstOrDefaultAsync(b => b.Name == "testuser");
            createdUser.IsActive = true;
            await service.SaveChanges(createdUser);

            var updatedUser = await context.Benutzer.FirstOrDefaultAsync(b => b.Name == "testuser");
            Assert.True(updatedUser.IsActive, "User changes were not saved.");
        }
    }
}
