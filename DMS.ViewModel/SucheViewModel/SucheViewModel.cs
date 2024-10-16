using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DMS.Model;
using DMS.Service;
using ViewModel.Interface.Suche;

namespace DMS.ViewModel.SucheViewModel
{
    public class SucheViewModel : ViewModelBase, ISucheViewModel
    {
        public event EventHandler<Ordner> FolderOpened;
        private readonly OrdnerService _ordnerService;
        private readonly DokumenteService _dokumenteService;
        private Benutzer _currentUser;
        private string _searchQuery;

        public ObservableCollection<SearchResultItem> SearchResults { get; set; }
        public ICommand SearchCommand { get; }
        public DelegateCommand OpenItemCommand { get; }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
            }
        }

        private SearchResultItem _selectedItem;
        public SearchResultItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        public SucheViewModel(OrdnerService ordnerService, DokumenteService dokumenteService)
        {
            _ordnerService = ordnerService;
            _dokumenteService = dokumenteService;
            SearchResults = [];
            SearchCommand = new DelegateCommand(ExecuteSearch);
            OpenItemCommand = new DelegateCommand(OnItemDoubleClick);
        }

        public void Init(Benutzer currentUser)
        {
            _currentUser = currentUser;
        }

        private async void ExecuteSearch(object obj)
        {
            SearchResults.Clear();

            var folders = await _ordnerService.GetFoldersAsync();
            var documents = await _dokumenteService.GetAllDocumentsAsync();

            var matchingFolders = folders
                .Where(f => f.Name.Contains(SearchQuery, StringComparison.InvariantCultureIgnoreCase));

            var matchingDocuments = documents
                .Where(d => d.Name.Contains(SearchQuery, StringComparison.InvariantCultureIgnoreCase) && (d.IsVisibleAllUser || d.Ersteller.Id == _currentUser.Id));

            foreach (var folder in matchingFolders)
            {
                SearchResults.Add(new SearchResultItem
                {
                    Name = folder.Name,
                    IsFolder = true,
                    Folder = folder,
                    Icon = "pack://application:,,,/Assets/folder-icon.png"
                });
            }

            foreach (var document in matchingDocuments)
            {
                SearchResults.Add(new SearchResultItem
                {
                    Name = document.Name,
                    IsFolder = false,
                    Document = document,
                    Icon = "pack://application:,,,/Assets/file-icon.png"
                });
            }
            var test = SearchResults.ToList();
            var x = test;
        }

        private void OnItemDoubleClick(object obj)
        {
            if (obj is SearchResultItem selectedItem)
            {
                Ordner folderToOpen = selectedItem.IsFolder ? selectedItem.Folder : selectedItem.Document.Ordner;
                FolderOpened?.Invoke(this, folderToOpen);
            }
        }
    }

    public class SearchResultItem
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public bool IsFolder { get; set; }
        public Ordner Folder { get; set; }
        public Dokument Document { get; set; }
    }
}
