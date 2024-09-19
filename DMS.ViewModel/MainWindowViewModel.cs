using DMS.Model;
using ViewModel.Interface;
using ViewModel.Interface.Login;

namespace DMS.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    private ILoginVM m_loginVM;
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

    public MainWindowViewModel(ILoginVM login)
    {
        m_loginVM = login;
        Init();
    }

    private void Init()
    {
        CurrentView = m_loginVM;
        m_loginVM.LoginSuccsess += ChangeCurrentUser;
    }

    private void ChangeCurrentUser(object sender, Benutzer currentuser)
    {
        CurrentUser = currentuser;
    }
}