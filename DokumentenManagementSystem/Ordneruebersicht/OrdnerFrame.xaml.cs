using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DMS.Model;
using DMS.ViewModel.Ordneruebersicht;

namespace DMS.View.Ordneruebersicht
{
    public partial class OrdnerFrame : UserControl
    {
        private TextBox? _lastFocusedTextBox;

        public OrdnerFrame()
        {
            InitializeComponent();

            var viewModel = DataContext as OrdnerFrameVM;
            if (viewModel != null)
            {
                viewModel.FolderCreated += OnFolderCreated;
            }
        }

        private async void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.DataContext is Ordner folder)
            {
                var viewModel = DataContext as OrdnerFrameVM;
                if (viewModel != null)
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
                if (textBox == _lastFocusedTextBox && textBox.IsFocused)
                    e.Handled = false;
                else
                {
                    // If the TextBox is not focused, select all text
                    textBox.Focus();
                    textBox.SelectAll();
                    _lastFocusedTextBox = textBox;
                    e.Handled = true;  // Mark the event as handled to prevent double clicks
                }
            }
        }

        private void OnUserControlMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_lastFocusedTextBox != null)
            {
                Keyboard.ClearFocus(); 
            }
        }
    }

}
