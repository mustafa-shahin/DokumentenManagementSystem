using DMS.Model;

namespace ViewModel.Interface.Login;

public interface ILoginVM : IViewModelBase
{
    event EventHandler<Benutzer> LoginEvent;
    void Init();
}