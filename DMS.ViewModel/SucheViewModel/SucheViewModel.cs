using System.Collections.ObjectModel;
using System.Windows.Input;
using DMS.Model;
using DMS.Service;
using ViewModel.Interface.Suche;

namespace DMS.ViewModel.SucheViewModel
{
    public class SucheViewModel : ViewModelBase, ISucheViewModel
    {
        private readonly SearchService _searchService;
        private Benutzer _currentUser;
        public event EventHandler<Ordner> FolderOpened;
        public event EventHandler<Benutzer> BenutzerOpened; 

        public ObservableCollection<SearchResultItem> SearchResults { get; }
        public ICommand SearchCommand { get; }
        public DelegateCommand OpenItemCommand { get; }

        private string _searchQuery;
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

        public SucheViewModel(SearchService searchService, Benutzer currentUser)
        {
            _searchService = searchService;
            SearchResults = new ObservableCollection<SearchResultItem>();
            _currentUser = currentUser;
            SearchCommand = new DelegateCommand(ExecuteSearch);
            OpenItemCommand = new DelegateCommand(OnItemDoubleClick);
        }

        public void Init(Benutzer currentUser)
        {
            _currentUser = currentUser;
        }

        private async void ExecuteSearch(object obj)
        {
            var results = await _searchService.Search(SearchQuery, _currentUser);

            SearchResults.Clear();
            foreach (var item in results)
            {
                SearchResults.Add(item);
            }
        }

        private void OnItemDoubleClick(object obj)
        {
            if (obj is SearchResultItem selectedItem)
            {
                if (selectedItem.Benutzer != null)
                    BenutzerOpened?.Invoke(this, selectedItem.Benutzer);
                else
                {
                    Ordner folderToOpen = selectedItem.IsFolder ? selectedItem.Folder : selectedItem.Document.Ordner;
                    FolderOpened?.Invoke(this, folderToOpen);
                }
            }
        }
    }
}
