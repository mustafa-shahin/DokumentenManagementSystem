using System.Windows.Input;
using DMS.Model;

namespace ViewModel.Interface.Suche
{
    public interface ISucheViewModel : IViewModelBase
    {
        event EventHandler<Ordner> FolderOpened;
        void Init(Benutzer currentUser);
        event EventHandler<Benutzer> BenutzerOpened;
        string SearchQuery { get; set; }
        ICommand SearchCommand { get; }
    }
}
