using DMS.Model;
using DMS.Service;
using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;
using System.IO;
using DMS.DataAccess;
using DokumentenManagementSystem.UI_Behavior;
using System.Windows.Input;
using DMS.ViewModel.Dokumentenuebersicht;
namespace DokumentenManagementSystem.Dokumentenuebersicht
{
    /// <summary>
    /// Interaction logic for EditFileDialog.xaml
    /// </summary>
    public partial class EditFileDialog : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly DokumenteService _dokumenteService;
        private string _fileDescription;
        private string _fileName;
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
        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                OnPropertyChanged(nameof(FileName));
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

        public EditFileDialog(Dokument file, DokumenteService dokumenteService)
        {
            InitializeComponent();
            _dokumenteService = dokumenteService;
            this.file = file;
            DataContext = this;
            FileName = file.Name;
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
                var newFileContent = File.ReadAllBytes(filePath);

                var newFileName = Path.GetFileName(filePath);

                if (double.TryParse(file.Version, out double currentVersion))
                {
                    file.Version = (currentVersion + 1).ToString("F1");
                }
                else
                {
                    file.Version = "1.0";
                }

                file.Content = newFileContent;

                file.Name = newFileName; 
                FileName = newFileName;   

                _dokumenteService.UpdateFile(file);

                MessageBox.Show("Die Datei wurde erfolgreich überschrieben und die Version aktualisiert!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Keine Datei wurde ausgewählt.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void OverrideFile_Click(object sender, RoutedEventArgs e)
        {
            FileOverride(this.file);
        }

        private void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            HoverEffect.ResizeOnHover(sender, e);
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            HoverEffect.ResizeOnHover(sender, e);
        }
    }
}