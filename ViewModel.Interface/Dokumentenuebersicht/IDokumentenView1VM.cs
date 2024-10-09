using DMS.Model;
using ViewModel.Interface;

namespace DMS.ViewModel.Dokumentenuebersicht
{
    public interface IDokumentenView1VM : IViewModelBase
    {
        void Init(Ordner folder, Benutzer currentUser);
    }
}
