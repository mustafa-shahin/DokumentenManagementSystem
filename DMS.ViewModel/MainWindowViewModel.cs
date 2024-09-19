using DMS.Model;
using DMS.ViewModel.Ordneruebersicht;
using ViewModel.Interface;
using ViewModel.Interface.Login;

namespace DMS.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    private ILoginVM m_loginVM;
    private IMainFrameVM m_mainFrameVM;
    private IViewModelBase m_currentView;

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

    public MainWindowViewModel(ILoginVM login, IMainFrameVM mainFrameVM)
    {
        m_loginVM = login;
        m_mainFrameVM = mainFrameVM;
        Init();
    }

    private void Init()
    {
        CurrentView = m_loginVM;
        m_loginVM.LoginEvent += LoginUser;
        m_mainFrameVM.LogoutEvent += LogoutUser;
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
}