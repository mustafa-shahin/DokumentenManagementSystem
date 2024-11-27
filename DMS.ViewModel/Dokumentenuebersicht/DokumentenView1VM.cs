using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using DMS.Model;
using DMS.Service;
using Microsoft.Win32;
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
        public bool IsEditingFile {  get; set; } = false;
        public bool DownloadDialog { get; set; } = false;

        public DokumenteService DokumenteService
        {
            get { return _dokumenteService; }
        }

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

        private  void AddFile(object obj)
        {
            var openFileDialog = new OpenFileDialog();
            var path = openFileDialog.ShowDialog() == true ? openFileDialog.FileName : string.Empty;
            _dokumenteService.AddFile(path, m_currentFolder, m_currentUser);
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

        private void OnEditFile(object obj)
        {
            IsEditingFile = true;

            if (obj is Dokument file)
            {
                EditFile?.Invoke(file);
            }
            SaveFile(obj);
        }
        private void DownloadFile(object obj)
        {
            DownloadDialog = true;

            if (obj is Dokument file)
            {
                var saveFileDialog = new SaveFileDialog
                {
                    FileName = file.Name
                };
                string savePath = saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : string.Empty;
                if (!string.IsNullOrEmpty(savePath))
                {
                    try
                    {
                        File.WriteAllBytes(savePath, file.Content);
                        FileDownloaded?.Invoke(savePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fehler beim Herunterladen der Datei: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
