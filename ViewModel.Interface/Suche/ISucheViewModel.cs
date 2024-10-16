using System.Collections.ObjectModel;
using System.Windows.Input;
using DMS.Model;

namespace ViewModel.Interface.Suche
{
    public interface ISucheViewModel : IViewModelBase
    {
        event EventHandler<Ordner> FolderOpened;
        void Init(Benutzer currentUser);

        string SearchQuery { get; set; }
        ICommand SearchCommand { get; }
    }
}
