using DMS.Model;
using DMS.ViewModel;
using DMS.ViewModel.Dokumentenuebersicht;
using DokumentenManagementSystem.Dokumentenuebersicht;
using DokumentenManagementSystem.UI_Behavior;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DMS.View.Dokumentenuebersicht
{
    /// <summary>
    /// Interaktionslogik für DokumentenView1.xaml
    /// </summary>
    public partial class DokumentenView1 : UserControl
    {
        private DokumentenView1VM _viewModel;
        private bool _isSidebarOpen = false;
        private bool _edit = false;
        public DokumentenView1()
        {
            InitializeComponent();
            this.Loaded += DokumentenView1_Loaded;
            _edit = true;

        }

        private void DokumentenView1_Loaded(object sender, RoutedEventArgs e)
        {
            SidebarCanvas.IsHitTestVisible = false;
            _viewModel = DataContext as DokumentenView1VM;

            if (_viewModel != null)
            {
                _viewModel.FileDownloaded += OnFileDownloaded;  
                    _viewModel.EditFile += OnEditFile;               
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
            if(_viewModel.DownloadDialog)
                dialog.ShowDialog();

            _viewModel.DownloadDialog = false;
        }
        private void OpenSidebar()
        {
            SidebarCanvas.IsHitTestVisible = true;
            _isSidebarOpen = true;
            SidebarCanvas.Visibility = Visibility.Visible;
            var storyboard = new Storyboard();

            var slideIn = new DoubleAnimation
            {
                From = -300,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.4),
                EasingFunction = new SineEase()
            };

            Storyboard.SetTarget(slideIn, FileInfoSidebar);
            Storyboard.SetTargetProperty(slideIn, new PropertyPath("(Canvas.Right)"));
            storyboard.Children.Add(slideIn);
            storyboard.Begin();
        }

        private void CloseSidebar()
        {
            if (_isSidebarOpen)
            {
                _isSidebarOpen = false;
                var storyboard = new Storyboard();

                var slideOut = new DoubleAnimation
                {
                    From = 0,
                    To = -300,
                    Duration = TimeSpan.FromSeconds(0.4),
                    EasingFunction = new SineEase()
                };

                Storyboard.SetTarget(slideOut, FileInfoSidebar);
                Storyboard.SetTargetProperty(slideOut, new PropertyPath("(Canvas.Right)"));
                storyboard.Children.Add(slideOut);

                slideOut.Completed += (s, e) => SidebarCanvas.Visibility = Visibility.Collapsed;

                storyboard.Begin();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_isSidebarOpen)
                CloseSidebar();
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenSidebar();
        }

        private void StackPanel_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is DokumentenView1VM viewModel)
            {
                if (sender is StackPanel stackPanel)
                {
                    if (stackPanel.Name == "upload")
                    {
                        if (viewModel.AddFileCommand.CanExecute(null))
                            viewModel.AddFileCommand.Execute(null);


                    }
                }
            }
        }
        private void OnEditFile(Dokument file)
        {
            if (_viewModel == null || !_viewModel.IsEditingFile)
                return;

            _viewModel.IsEditingFile = true;

            var editDialog = new EditFileDialog(file);
            var result = editDialog.ShowDialog();

            if (result == true)
            {
                file.Description = editDialog.FileDescription;
                file.IsVisibleAllUser = editDialog.IsVisibleAllUser;
                _viewModel.SaveFileCommand.Execute(file);
                _viewModel.LoadFiles();
            }

            _viewModel.IsEditingFile = false;

        }
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            HoverEffect.ResizeOnHover(sender, e);
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            HoverEffect.ResizeOnHover(sender, e);
        }

    }
}