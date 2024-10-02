using DMS.DataAccess;
using DMS.Model;
using DMS.Service;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DMS.Tests
{
    public class OrdnerServiceTests
    {
        private DataContext GetCreateTestDbContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new DataContext(options); 
        }

        [Fact]
        public async Task OrdnerService_ShouldCreateFolder()
        {
            // Arrange
            var context = GetCreateTestDbContext();
            var service = new OrdnerService(context);

            // Act
            var folder = await service.CreateFolderAsync();

            // Assert
            Assert.NotNull(folder);
            Assert.Equal("Neuer Ordner", folder.Name);
            Assert.Equal(1, folder.Order);
        }

        [Fact]
        public async Task OrdnerService_ShouldUpdateFolder()
        {
            // Arrange
            var context = GetCreateTestDbContext();
            var service = new OrdnerService(context);
            var folder = await service.CreateFolderAsync();

            // Act
            folder.Name = "Updated Folder";
            await service.UpdateFolderAsync(folder);
            var updatedFolder = await context.Ordner.FindAsync(folder.Id);

            // Assert
            Assert.NotNull(updatedFolder);
            Assert.Equal("Updated Folder", updatedFolder.Name);
        }

        [Fact]
        public async Task OrdnerService_ShouldGetFolders()
        {
            // Arrange
            var context = GetCreateTestDbContext();
            var service = new OrdnerService(context);

            // Act
            await service.CreateFolderAsync();
            var folders = await service.GetFoldersAsync();

            // Assert
            Assert.NotEmpty(folders);
            Assert.Single(folders); 
        }

        [Fact]
        public async Task OrdnerService_ShouldReturnCorrectFolderOrder()
        {
            // Arrange
            var context = GetCreateTestDbContext();
            var service = new OrdnerService(context);

            // Act
            await service.CreateFolderAsync();
            await service.CreateFolderAsync();
            var folders = await service.GetFoldersAsync();

            // Assert
            Assert.Equal(2, folders.Count); 
            Assert.Equal(1, folders[0].Order);
            Assert.Equal(2, folders[1].Order);
        }
    }
}
