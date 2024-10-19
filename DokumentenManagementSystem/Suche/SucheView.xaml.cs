using DMS.Service;
using DMS.ViewModel.SucheViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace DokumentenManagementSystem.Suche
{
    /// <summary>
    /// Interaction logic for SucheView.xaml
    /// </summary>
    public partial class SucheView : UserControl
    {
        public SucheView()
        {
            InitializeComponent();
        }
        private void OnResultDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is SucheViewModel viewModel)
            {
                var listView = (ListView)sender;
                if (listView.SelectedItem is SearchResultItem selectedItem && viewModel.OpenItemCommand.CanExecute(selectedItem))
                {
                    viewModel.OpenItemCommand.Execute(selectedItem);
                }
            }
        }
    }
}