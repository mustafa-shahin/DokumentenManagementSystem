using DMS.DataAccess;
using DMS.Model;
using DMS.Service;
using Microsoft.EntityFrameworkCore;

namespace DMS.Tests
{
    public class DokumenteServiceTests
    {
        private DataContext GetCreateTestDbContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;
            return new DataContext(options);
        }
        [Fact]
        public async Task DokumenteService_ShouldAddFileSuccessfully()
        {
            // Arrange
            var context = GetCreateTestDbContext();
            var service = new DokumenteService(context);

            var folder = new Ordner { Id = 1, Name = "TestFolder", SearchTags = [] };
            var user = new Benutzer { Id = 1, Name = "TestUser", Passwort = "12345678" };

            // Add necessary entities to in-memory database
            await context.Ordner.AddAsync(folder);
            await context.Benutzer.AddAsync(user);
            await context.SaveChangesAsync();

            // Mock the file path and content
            var filePath = "testfile.txt";
            var fileContent = new byte[] { 0x01, 0x02, 0x03 };
            File.WriteAllBytes(filePath, fileContent);

            try
            {
                // Act
                service.AddFile(filePath, folder, user);

                // Assert
                var createdFile = await context.Dokumente.FirstOrDefaultAsync(d => d.Name == "testfile.txt");
                Assert.NotNull(createdFile);
                Assert.Equal("testfile.txt", createdFile.Name);
                Assert.Equal(fileContent, createdFile.Content);
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }



        [Fact]
        public async Task DokumenteService_ShouldUpdateFileSuccessfully()
        {
            // Arrange
            var context = GetCreateTestDbContext();
            var service = new DokumenteService(context);

            var folder = new Ordner { Id = 1, Name = "TestFolder" };
            var user = new Benutzer { Id = 1, Name = "TestUser",  Passwort = "12345678" };

            var dokument = new Dokument
            {
                Id = 1,
                Name = "testfile.txt",
                Description = "Initial description",
                Content = new byte[] { 0x01, 0x02 },
                Ersteller = user,
                Version = "1.0",
                OrdnerId = folder.Id
               
            };

            await context.Dokumente.AddAsync(dokument);
            await context.SaveChangesAsync();

            // Act
            dokument.Description = "Updated description";
            dokument.Content = [0x03, 0x04];
            service.UpdateFile(dokument);

            // Assert
            var updatedFile = await context.Dokumente.FirstOrDefaultAsync(d => d.Id == dokument.Id);
            Assert.NotNull(updatedFile);
            Assert.Equal("Updated description", updatedFile.Description);
            Assert.Equal(new byte[] { 0x03, 0x04 }, updatedFile.Content);
        }

        [Fact]
        public async Task DokumenteService_ShouldRetrieveFilesForFolder()
        {
            // Arrange
            var context = GetCreateTestDbContext();
            var service = new DokumenteService(context);

            var folder = new Ordner { Id = 1, Name = "TestFolder", SearchTags = [] };
            var user = new Benutzer { Id = 1, Name = "TestUser", Passwort = "12345678" };

            var dokument = new Dokument
            {
                Id = 1,
                Name = "testfile.txt",
                Description = "Initial description",
                Content = [0x01, 0x02],
                Ersteller = user,
                Version = "1.0",
                OrdnerId = folder.Id
            };

            await context.Ordner.AddAsync(folder);
            await context.Benutzer.AddAsync(user);
            await context.Dokumente.AddAsync(dokument);
            await context.SaveChangesAsync();

            // Act
            var files = await service.GetFilesForFolder(folder.Id, user);

            // Assert
            Assert.Single(files);
            Assert.Equal("testfile.txt", files[0].Name);
        }

        [Fact]
        public async Task DokumenteService_ShouldGetAllDocumentsSuccessfully()
        {
            // Arrange
            var context = GetCreateTestDbContext();
            var service = new DokumenteService(context);
            var folder = new Ordner { Id = 1, Name = "TestFolder", SearchTags = [] };
            var user = new Benutzer { Id = 1, Name = "TestUser", Passwort = "12345678" };

            var dokument1 = new Dokument
            {
                Id = 1,
                Name = "doc1.txt",
                Description = "Initial description",
                Content = [0x01, 0x02],
                Ersteller = user,
                Version = "1.0",
                OrdnerId = folder.Id,
                Ordner = folder
            };
            var dokument2= new Dokument
            {
                Id = 2,
                Name = "doc2.txt",
                Description = "Initial description",
                Content = [0x01, 0x02],
                Ersteller = user,
                Version = "1.0",
                OrdnerId = folder.Id,
                Ordner = folder
            };

            await context.Dokumente.AddRangeAsync(dokument1, dokument2);
            await context.SaveChangesAsync();

            // Act
            var documents = await service.GetAllDocuments();

            // Assert
            Assert.Equal(2, documents.Count);
            Assert.Contains(documents, d => d.Name == "doc1.txt");
            Assert.Contains(documents, d => d.Name == "doc2.txt");
        }
    }
}
