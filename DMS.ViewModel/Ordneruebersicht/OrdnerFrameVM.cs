using DMS.Model;
using DMS.Service;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ViewModel.Interface;

namespace DMS.ViewModel.Ordneruebersicht
{
    public class OrdnerFrameVM : ViewModelBase, IOrdnerFrameVM
    {
        private readonly IOrdnerView1VM m_ordnerView1VM;
        private readonly OrdnerService m_ordnerService;
        private IViewModelBase m_currentView;

        public IViewModelBase CurrentView
        {
            get => m_currentView;
            set
            {
                if (Equals(value, m_currentView)) return;
                m_currentView = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Ordner> OrdnerCollection { get; set; }
        public event EventHandler<Ordner> FolderCreated;

        public DelegateCommand CreateFolderCommand { get; set; }

        public OrdnerFrameVM(IOrdnerView1VM ordnerView1VM, OrdnerService ordnerService)
        {
            m_ordnerService = ordnerService;
            m_ordnerView1VM = ordnerView1VM;
            OrdnerCollection = [];

            CreateFolderCommand = new DelegateCommand(OnCreateFolder);
            LoadFoldersAsync();
        }

        public void Init(Benutzer currentUser)
        {
            m_ordnerView1VM.Init(currentUser);
            CurrentView = m_ordnerView1VM;
        }

        private async void OnCreateFolder(object? o)
        {
            var newFolder = await m_ordnerService.CreateFolderAsync(); 
            OrdnerCollection.Add(newFolder);  
            FolderCreated?.Invoke(this, newFolder); 
        }

        // Load folders by calling OrdnerService
        private async void LoadFoldersAsync()
        {
            var folders = await m_ordnerService.GetFoldersAsync();
            foreach (var folder in folders)
            {
                OrdnerCollection.Add(folder);
            }
        }

        public async Task SaveFolderChangesAsync(Ordner folder)
        {
            if (!string.IsNullOrWhiteSpace(folder.Name))
            {
                await m_ordnerService.UpdateFolderAsync(folder); 
            }
        }
    }
}
