namespace ViewModel.Interface.ForgotPassword
{
    public interface IForgotPasswordVM  : IViewModelBase
    {
        event EventHandler PasswordChangedEvent;
        event EventHandler GoBackEvent;
        void Init();
    }
}
