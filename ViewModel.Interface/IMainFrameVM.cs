using DMS.Model;

namespace ViewModel.Interface;

public interface IMainFrameVM : IViewModelBase
{
    event EventHandler LogoutEvent;
    void Init(Benutzer benutzer);
}