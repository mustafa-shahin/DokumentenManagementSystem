using DMS.Model;
using System.Windows.Input;
namespace ViewModel.Interface.Login;

public interface ILoginVM : IViewModelBase
{
    event EventHandler<Benutzer> LoginEvent;
    ICommand ForgotPasswordCommand { get; }
    event EventHandler ForgotPasswordEvent;
    void Init();
}