using DMS.ViewModel;
using DMS.ViewModel.SucheViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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