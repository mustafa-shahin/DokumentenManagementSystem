using DMS.DataAccess;
using DMS.Model;
using DMS.Service;
using Microsoft.EntityFrameworkCore;

namespace DMS.Tests
{
    public class SearchServiceTests
    {
        private readonly DataContext _context;
        private readonly DokumenteService _dokumenteService;
        private readonly OrdnerService _ordnerService;
        private readonly BenutzerService _benutzerService;
        private readonly SearchService _searchService;

        public SearchServiceTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            _dokumenteService = new DokumenteService(_context);
            _ordnerService = new OrdnerService(_context);
            _benutzerService = new BenutzerService(_context);

            _searchService = new SearchService(_dokumenteService, _ordnerService, _benutzerService);

            SeedData();
        }

        private void SeedData()
        {
            var folder = new Ordner
            {
                Id = 1,
                Name = "TestFolder",
                SearchTags = []
            };
            _context.Ordner.Add(folder);

            var document = new Dokument
            {
                Id = 1,
                Name = "TestDocument",
                Description = "Initial description",
                Content = [0x01, 0x02],
                Version = "1.0",
                Ersteller = new Benutzer { Id = 2, Name = "TestUser", Passwort = "password123", IsActive = true },
                IsVisibleAllUser = true,
                Ordner = folder
            };
            _context.Dokumente.Add(document);

            var user = new Benutzer
            {
                Id = 1,
                Name = "AdminUser",
                IsAdmin = true,
                Passwort = "adminpass",
                IsActive = true
            };
            _context.Benutzer.Add(user);

            _context.SaveChanges();
        }

        [Fact]
        public async Task Search_ShouldReturnMultipleMatchingResults()
        {
            // Arrange
            var searchQuery = "Test";
            var adminUser = _context.Benutzer.First(u => u.IsAdmin);

            // Act
            var results = await _searchService.Search(searchQuery, adminUser);

            // Assert
            Assert.Contains(results, item =>
                item.Name == "TestFolder" &&
                item.Type == "Ordner" &&
                item.IsFolder == true &&
                item.Folder != null &&
                item.Folder.Name == "TestFolder"
            );

            Assert.Contains(results, item =>
                item.Name == "TestDocument" &&
                item.Type == "Dokument" &&
                item.IsFolder == false &&
                item.Document != null &&
                item.Document.Name == "TestDocument"
            );

            Assert.Contains(results, item =>
                item.Name == "TestUser" &&
                item.Type == "Benutzer" &&
                item.IsFolder == false &&
                item.Benutzer != null &&
                item.Benutzer.Name == "TestUser"
            );
        }
    }

}
