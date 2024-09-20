using DMS.Model;
using ViewModel.Interface;

namespace DMS.ViewModel.Ordneruebersicht;

public interface IOrdnerFrameVM : IViewModelBase
{
    void Init(Benutzer currentUser);
}