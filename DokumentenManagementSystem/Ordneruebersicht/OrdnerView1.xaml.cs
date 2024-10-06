using DMS.Model;
using DMS.ViewModel.Ordneruebersicht;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace DMS.View.Ordneruebersicht
{
    /// <summary>
    /// Interaktionslogik für OrdnerView1.xaml
    /// </summary>
    public partial class OrdnerView1 : UserControl
    {
        private TextBox? _lastFocusedTextBox;
        public OrdnerView1()
        {
            InitializeComponent();
            if (DataContext is OrdnerView1VM viewModel)
            {
                viewModel.FolderCreated += OnFolderCreated;
            }
            this.Loaded += OrdnerView1_Loaded;
            this.Unloaded += OrdnerView1_Unloaded;
        }
        private void OrdnerView1_Unloaded(object sender, RoutedEventArgs e)
        {
            Application.Current.Deactivated -= Application_Deactivated;
        }

        private void OrdnerView1_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.Deactivated += Application_Deactivated;
        }
        private async void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.DataContext is Ordner folder)
            {
                if (DataContext is OrdnerView1VM viewModel)
                {
                    await viewModel.SaveFolderChangesAsync(folder);
                }

                _lastFocusedTextBox = null;
            }
        }
        private void OnFolderCreated(object sender, Ordner folder)
        {
            this.Dispatcher.InvokeAsync(() =>
            {
                foreach (var item in FolderItemsControl.Items)
                {
                    if (item is Ordner && item == folder)
                    {
                        var container = FolderItemsControl.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;

                        if (container?.FindName("FolderNameTextBox") is TextBox textBox)
                        {
                            textBox.Focus();
                            textBox.SelectAll();

                            _lastFocusedTextBox = textBox;
                        }
                        break;
                    }
                }
            });
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Keyboard.ClearFocus();
        }
        private void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (!textBox.IsFocused)
                {
                    textBox.Focus();
                    textBox.SelectAll();
                    _lastFocusedTextBox = textBox;
                    e.Handled = true; 
                }
                else
                {
                    e.Handled = false; 
                }
            }
            if (this.DataContext is OrdnerView1VM viewModel)
            {
                if (sender is TextBox TextBox && TextBox.DataContext is Ordner folder)
                {
                    viewModel.SelectedFolder = folder;
                }
            }
        }

        private void OnUserControlMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.DataContext is OrdnerView1VM viewModel && viewModel.SelectedFolder != null)
            {
                Keyboard.ClearFocus();
                _ = viewModel.SaveFolderChangesAsync(viewModel.SelectedFolder);
            }
        }
        private void Application_Deactivated(object sender, EventArgs e)
        {
            if (_lastFocusedTextBox != null)
            {
                var textBox = _lastFocusedTextBox;
                var binding = textBox.GetBindingExpression(TextBox.TextProperty);
                binding?.UpdateSource();

                if (textBox.DataContext is Ordner folder)
                {
                    if (DataContext is OrdnerView1VM viewModel)
                    {
                        _ = viewModel.SaveFolderChangesAsync(folder);
                    }
                }
                Keyboard.Focus(null);
                _lastFocusedTextBox = null;
            }
        }
        private void FolderIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (DataContext is OrdnerView1VM viewModel && sender is Image image && image.DataContext is Ordner folder)
                {
                    viewModel.OpenFolderCommand.Execute(folder);
                    e.Handled = true;
                }
            }
        }
    }
}
