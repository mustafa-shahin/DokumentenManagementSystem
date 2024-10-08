using DMS.ViewModel.Dokumentenuebersicht;
using DokumentenManagementSystem.Dokumentenuebersicht;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DMS.View.Dokumentenuebersicht
{
    /// <summary>
    /// Interaktionslogik für DokumentenView1.xaml
    /// </summary>
    public partial class DokumentenView1 : UserControl
    {
        private DokumentenView1VM _viewModel;

        public DokumentenView1()
        {
            InitializeComponent();
            this.Loaded += DokumentenView1_Loaded;
        }

        private void DokumentenView1_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as DokumentenView1VM;

            if (_viewModel != null)
            {
                // Subscribe to the FileDownloaded event
                _viewModel.FileDownloaded += OnFileDownloaded;
            }
        }

        private void DescriptionTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Beschreibung")
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.White;
            }
        }

        private void DescriptionTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Beschreibung";
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void OnFileDownloaded(string filePath)
        {
            var dialog = new DownloadCompleteDialog(filePath);
            dialog.ShowDialog();
        }
    }
}
