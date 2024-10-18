namespace ViewModel.Interface.ForgotPassword
{
    public interface IForgotPasswordVM  : IViewModelBase
    {
        event EventHandler PasswordChangedEvent;
        void Init();
    }
}
