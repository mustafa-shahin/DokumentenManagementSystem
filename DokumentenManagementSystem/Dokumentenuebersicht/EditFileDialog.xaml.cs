using DMS.Model;
using DMS.Service;
using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;
using DMS.Service;
using System.IO;
using DMS.DataAccess;
namespace DokumentenManagementSystem.Dokumentenuebersicht
{
    /// <summary>
    /// Interaction logic for EditFileDialog.xaml
    /// </summary>
    public partial class EditFileDialog : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private  DokumenteService _dokumenteService;
        private string _fileDescription;
        private Dokument file;
        public string FileDescription
        {
            get => _fileDescription;
            set
            {
                _fileDescription = value;
                OnPropertyChanged(nameof(FileDescription));
            }
        }

        private bool _isVisibleAllUser;
        public bool IsVisibleAllUser
        {
            get => _isVisibleAllUser;
            set
            {
                _isVisibleAllUser = value;
                OnPropertyChanged(nameof(IsVisibleAllUser));
            }
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public EditFileDialog(Dokument file, DokumenteService dokumenteService = null)
        {
            InitializeComponent();
            _dokumenteService = dokumenteService;
            this.file = file;
            DataContext = this;
            FileDescription = file.Description;
            IsVisibleAllUser = file.IsVisibleAllUser;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
        private void FileOverride(Dokument file)
        {

            var openFileDialog = new OpenFileDialog
            {
                Filter = "All Files (*.*)|*.*",
                Title = "Select a File to Override"
            };

            var filePath = openFileDialog.ShowDialog() == true ? openFileDialog.FileName : string.Empty;

            if (!string.IsNullOrEmpty(filePath))
            {
                // Read the new file content
                var newFileContent = File.ReadAllBytes(filePath);

                if (double.TryParse(file.Version, out double currentVersion))
                {
                    file.Version = (currentVersion + 1).ToString("F1"); 
                }
                else
                {
                    file.Version = "1.0"; 
                }

                file.Content = newFileContent;

                file.Description = $"Updated on {DateTime.Now}";

                _dokumenteService.UpdateFile(file);

                MessageBox.Show("File has been successfully overridden and version updated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No file selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OverrideFile_Click(object sender, RoutedEventArgs e)
        {
            FileOverride(this.file);
        }
    }
}