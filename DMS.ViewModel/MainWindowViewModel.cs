using DMS.Model;
using ViewModel.Interface;
using ViewModel.Interface.Login;
using DMS.Service;
using ViewModel.Interface.ForgotPassword;
namespace DMS.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    private ILoginVM m_loginVM;
    private IMainFrameVM m_mainFrameVM;
    private IViewModelBase m_currentView;
    private IForgotPasswordVM m_forgotPasswordVM;
    private Benutzer m_currentUser;

    public Benutzer CurrentUser
    {
        get => m_currentUser;
        set
        {
            if (Equals(value, m_currentUser)) return;
            m_currentUser = value;
            OnPropertyChanged();
        }
    }

    public IViewModelBase CurrentView
    {
        get => m_currentView;
        set
        {
            if (Equals(value, m_currentView)) return;
            m_currentView = value;
            OnPropertyChanged();
        }
    }

    public MainWindowViewModel(ILoginVM login, IMainFrameVM mainFrameVM, IForgotPasswordVM forgotPasswordVM)
    {
        m_loginVM = login;
        m_mainFrameVM = mainFrameVM;
        m_forgotPasswordVM = forgotPasswordVM;
        Init();
    }

    private void Init()
    {
        CurrentView = m_loginVM;
        m_loginVM.LoginEvent += LoginUser;
        m_mainFrameVM.LogoutEvent += LogoutUser;
        m_loginVM.ForgotPasswordEvent += OnForgotPassword;
    }

    private void LoginUser(object sender, Benutzer currentuser)
    {
        CurrentUser = currentuser;
        m_mainFrameVM.Init(currentuser);
        CurrentView = m_mainFrameVM;
    }

    private void LogoutUser(object sender, EventArgs e)
    {
        CurrentUser = null;
        m_loginVM.Init();
        CurrentView = m_loginVM;
    }
    private void OnForgotPassword(object sender, EventArgs e)
    {
        m_forgotPasswordVM.PasswordChangedEvent += OnPasswordChanged;
        m_forgotPasswordVM.Init();
        CurrentView = m_forgotPasswordVM;
    }
    private void OnPasswordChanged(object sender, EventArgs e)
    {
        m_loginVM.Init();
        CurrentView = m_loginVM;
    }
}