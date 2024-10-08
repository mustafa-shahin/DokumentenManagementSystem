using System.IO;
using System.Diagnostics;
using System.Windows;

namespace DokumentenManagementSystem.Dokumentenuebersicht
{
    /// <summary>
    /// Interaction logic for DownloadCompleteDialog.xaml
    /// </summary>
    public partial class DownloadCompleteDialog : Window
    {
        private readonly string _filePath;

        public DownloadCompleteDialog()
        {
            InitializeComponent();
        }

        public DownloadCompleteDialog(string filePath) // Constructor name corrected
        {
            InitializeComponent();
            _filePath = filePath;
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFile(_filePath);
            this.Close();
        }

        private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            OpenContainingFolder(_filePath);
            this.Close();
        }

        private static void OpenFile(string filePath)
        {
            try
            {
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Öffnen der Datei: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static void OpenContainingFolder(string filePath)
        {
            try
            {
                // Use global::System.IO.Path to avoid ambiguity with System.Windows.Shapes.Path
                string folderPath = Path.GetDirectoryName(filePath);
                Process.Start(new ProcessStartInfo("explorer.exe", $"/select,\"{filePath}\"") { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Öffnen des Ordners: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
