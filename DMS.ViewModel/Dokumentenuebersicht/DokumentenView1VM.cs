using System.Collections.ObjectModel;
using System.Windows.Controls;
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
        public event Action<string> FileDownloaded;
        public event Action<Dokument> EditFile;
        public ICommand EditFileCommand { get; set; }
        private Dokument _selectedFile = new();
        public bool _isEditingFile {  get; set; } = false;
        public Dokument SelectedFile
        {
            get => _selectedFile;
            set
            {
                _selectedFile = value;
                OnPropertyChanged();
            }
        }
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
            EditFileCommand = new DelegateCommand(OnEditFile);
            FilesCollection = [];
        }

        public void Init(Ordner folder, Benutzer currentUser)
        {
            if(FilesCollection.Any())
                FilesCollection.Clear();
            m_currentFolder = folder;
            m_currentUser = currentUser;
            LoadFiles();
        }

        public async void LoadFiles()
        {
            FilesCollection.Clear();
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
                _dokumenteService.DownloadFile(file, out bool openDialog);
                if(openDialog)
                    FileDownloaded?.Invoke(_dokumenteService.FilePath);
            }
        }
        private void OnEditFile(object obj)
        {
            if (_isEditingFile)
                return;

            _isEditingFile = true;

            if (obj is Dokument file)
            {
                EditFile?.Invoke(file);
            }
            SaveFile(obj);
        }
    }
}
