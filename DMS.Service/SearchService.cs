using DMS.Model;

namespace DMS.Service
{
    public class SearchService(DokumenteService dokumenteService, OrdnerService ordnerService, BenutzerService benutzerService)
    {
        private readonly DokumenteService _dokumenteService = dokumenteService;
        private readonly OrdnerService _ordnerService = ordnerService;
        private readonly BenutzerService _benutzerService = benutzerService;

        public async Task<IEnumerable<SearchResultItem>> Search(string searchQuery, Benutzer currentUser)
        {
            var searchResults = new List<SearchResultItem>();
            var folders = await _ordnerService.GetFolders();
            var documents = await _dokumenteService.GetAllDocuments();
            var users = await _benutzerService.GetAllUsers();

            var matchingFolders = folders
                .Where(f => f.Name.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase));

            var matchingDocuments = documents
                .Where(d => d.Name.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase) &&
                            (d.IsVisibleAllUser || d.Ersteller.Id == currentUser.Id));

            var matchingUsers = users
                    .Where(d => d.Name.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase));
            searchResults.AddRange(matchingFolders.Select(folder => new SearchResultItem
            {
                Name = folder.Name,
                IsFolder = true,
                Folder = folder,
                Icon = "pack://application:,,,/Assets/folder-icon.png",
                Type = "Ordner"
            }));

            searchResults.AddRange(matchingDocuments.Select(document => new SearchResultItem
            {
                Name = document.Name,
                IsFolder = false,
                Document = document,
                Icon = "pack://application:,,,/Assets/file-icon.png",
                Type = "Dokument"
            }));
            if (currentUser.IsAdmin) 
            {
                searchResults.AddRange(matchingUsers.Select(user => new SearchResultItem
                {
                    Name = user.Name,
                    IsFolder = false,
                    Benutzer = user,
                    Icon = "pack://application:,,,/Assets/user-icon.png",
                    Type = "Benutzer"
                }));
            }

            return searchResults;
        }
    }

    public class SearchResultItem
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public bool IsFolder { get; set; }
        public Ordner Folder { get; set; }
        public Dokument Document { get; set; }
        public Benutzer Benutzer { get; set; }
        public string Type { get; set; }
    }
}
