using DMS.Model;
using DMS.ViewModel.Nutzerverwaltung;
using DMS.ViewModel.Ordneruebersicht;
using ViewModel.Interface;

namespace DMS.ViewModel;

public class MainFrameVM : ViewModelBase, IMainFrameVM
{
    private Benutzer m_currentUser;
    private IOrdnerFrameVM m_ordnerFrameVM;
    private INutzerFrameVM m_nutzerFrameVM;

    private IViewModelBase m_currentView;

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

    public DelegateCommand OrdnerCommand { get; set; }
    public DelegateCommand NutzerCommand { get; set; }
    public DelegateCommand LogoutCommand { get; set; }

    public event EventHandler? LogoutEvent;

    public MainFrameVM(IOrdnerFrameVM ordnerFrameVM, INutzerFrameVM nutzerFrameVM)
    {
        m_ordnerFrameVM = ordnerFrameVM;
        m_nutzerFrameVM = nutzerFrameVM;

        OrdnerCommand = new DelegateCommand(OnOrdner);
        NutzerCommand = new DelegateCommand(OnNutzer);
        LogoutCommand = new DelegateCommand(OnLogout);
    }

    public void Init(Benutzer currentUser)
    {
        CurrentUser = currentUser;
        CurrentView = m_ordnerFrameVM;
    }

    private void OnOrdner(object? o)
    {
        m_ordnerFrameVM.Init(CurrentUser);
        CurrentView = m_ordnerFrameVM;
    }

    private void OnNutzer(object? o)
    {
        m_nutzerFrameVM.Init(CurrentUser);
        CurrentView = m_nutzerFrameVM;
    }

    private void OnLogout(object? o)
    {
        LogoutEvent.Invoke(this, EventArgs.Empty);
    }
}