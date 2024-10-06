using DMS.Model;
using ViewModel.Interface;

namespace DMS.ViewModel.Ordneruebersicht;

public interface IOrdnerView1VM : IViewModelBase
{
    event EventHandler<Ordner> FolderOpened;
    void Init(Benutzer currentUser);
}