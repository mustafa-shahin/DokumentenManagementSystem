using System.Collections.ObjectModel;
using System.Windows.Input;
using DMS.Model;
using DMS.Service;

namespace DMS.ViewModel.Dokumentenuebersicht
{
    public class DokumentenView1VM : ViewModelBase, IDokumentenView1VM
    {
        private Ordner m_currentFolder;
        private Benutzer m_currentUser;
        private readonly DokumenteService _dokumenteService;

        public ObservableCollection<Dokument> FilesCollection { get; set; }

        public ICommand AddFileCommand { get; set; }
        public ICommand SaveFileCommand { get; set; }
        public ICommand DownloadFileCommand { get; set; }

        public string FolderName
        {
            get => m_currentFolder?.Name;
            private set
            {
                if (m_currentFolder != null)
                {
                    m_currentFolder.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public DokumentenView1VM(DokumenteService dokumenteService)
        {
            _dokumenteService = dokumenteService;
            AddFileCommand = new DelegateCommand(AddFile);
            SaveFileCommand = new DelegateCommand(SaveFile);
            DownloadFileCommand = new DelegateCommand(DownloadFile);
            FilesCollection = [];
        }

        public void Init(Ordner folder, Benutzer currentUser)
        {
            m_currentFolder = folder;
            m_currentUser = currentUser;
            LoadFiles();
        }

        private async void LoadFiles()
        {
            var files = await _dokumenteService.GetFilesForFolder(m_currentFolder.Id, m_currentUser);
            foreach (var file in files)
            {
                FilesCollection.Add(file);
            }
        }

        private void AddFile(object obj)
        {
            // Call service to add file using OpenFileDialog
            _dokumenteService.AddFile(m_currentFolder, m_currentUser);

            // Refresh the FilesCollection after adding the file
            FilesCollection.Clear();
            LoadFiles();
        }

        private void SaveFile(object obj)
        {
            if (obj is Dokument file)
            {
                _dokumenteService.UpdateFile(file);
            }
        }

        private void DownloadFile(object obj)
        {
            if (obj is Dokument file)
            {
                DokumenteService.DownloadFile(file);
            }
        }
    }
}
