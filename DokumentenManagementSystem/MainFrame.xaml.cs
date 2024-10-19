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
    }
}