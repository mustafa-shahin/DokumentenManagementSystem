using DMS.Model;
using System.ComponentModel;
using System.Windows;

namespace DokumentenManagementSystem.Dokumentenuebersicht
{
    /// <summary>
    /// Interaction logic for EditFileDialog.xaml
    /// </summary>
    public partial class EditFileDialog : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _fileDescription;
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

        public EditFileDialog(Dokument file)
        {
            InitializeComponent();
            DataContext = this;
            FileDescription = file.Description;
            IsVisibleAllUser = file.IsVisibleAllUser;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}