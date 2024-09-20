using DMS.Model;
using ViewModel.Interface;

namespace DMS.ViewModel.Nutzerverwaltung;

public interface INutzerFrameVM : IViewModelBase
{
    void Init(Benutzer currentUser);
}