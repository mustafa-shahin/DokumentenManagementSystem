using DMS.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace DokumentenManagementSystem
{
    /// <summary>
    /// Interaktionslogik für MainFrame.xaml
    /// </summary>
    public partial class MainFrame : UserControl
    {
        public MainFrame()
        {
            InitializeComponent();
        }
        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if(sender is TextBox)
                {
                    if(!string.IsNullOrEmpty((DataContext as MainFrameVM)?.SearchQuery))
                        (DataContext as MainFrameVM).SearchQuery = (sender as TextBox).Text;
                }
                if (DataContext is MainFrameVM viewModel)
                {
                    if (viewModel.SearchCommand.CanExecute(null))
                    {
                        viewModel.SearchCommand.Execute(null);
                    }
                }
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is MainFrameVM viewModel)
            {
                if (sender is TextBlock textBlock)
                {
                    if (textBlock.Name == "users")
                    {
                        if (viewModel.NutzerCommand.CanExecute(null))
                            viewModel.NutzerCommand.Execute(null);
                    }
                    else if (textBlock.Name == "folders")
                    {
                        if (viewModel.OrdnerCommand.CanExecute(null))
                            viewModel.OrdnerCommand.Execute(null);
                    }
                    else if (textBlock.Name == "logout")
                    {
                        if (viewModel.LogoutCommand.CanExecute(null))
                            viewModel.LogoutCommand.Execute(null);
                    }
                }
            }
        }


        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is MainFrameVM viewModel)
            {
                if (sender is StackPanel stackPanel)
                {
                    if (stackPanel.Name == "users")
                    {
                        if (viewModel.NutzerCommand.CanExecute(null))
                            viewModel.NutzerCommand.Execute(null);
                    }
                    else if (stackPanel.Name == "folders")
                    {
                        if (viewModel.OrdnerCommand.CanExecute(null))
                            viewModel.OrdnerCommand.Execute(null);
                    }
                    else if (stackPanel.Name == "logout")
                    {
                        if (viewModel.LogoutCommand.CanExecute(null))
                            viewModel.LogoutCommand.Execute(null);
                    }
                }
            }
        }
    }
}