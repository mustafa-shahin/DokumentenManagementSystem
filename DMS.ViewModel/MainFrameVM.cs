using DMS.Model;
using DMS.ViewModel.Dokumentenuebersicht;
using DMS.ViewModel.Nutzerverwaltung;
using DMS.ViewModel.Ordneruebersicht;
using DMS.ViewModel.ProfileVM;
using ViewModel.Interface;
using ViewModel.Interface.Profile;
using ViewModel.Interface.Suche;

namespace DMS.ViewModel;

public class MainFrameVM : ViewModelBase, IMainFrameVM
{
    private Benutzer m_currentUser;
    private IOrdnerFrameVM m_ordnerFrameVM;
    private INutzerFrameVM m_nutzerFrameVM;
    private ISucheViewVM m_sucheViewModel;
    private IDokumentenFrameVM m_dokumentenFrameVM;
    private IViewModelBase m_currentView;
    private IProfileViewVM m_profileVM;
    private string m_searchQuery;

    public string SearchQuery
    {
        get => m_searchQuery;
        set
        {
            if (Equals(value, m_searchQuery)) return;
            m_searchQuery = value;
            OnPropertyChanged();
        }
    }

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
    public DelegateCommand SearchCommand { get; set; }
    public event EventHandler? LogoutEvent;
    public DelegateCommand ProfileCommand { get; set; }

    public MainFrameVM(IOrdnerFrameVM ordnerFrameVM, INutzerFrameVM nutzerFrameVM, ISucheViewVM sucheViewModel, IDokumentenFrameVM dokumentenFrameVM, IProfileViewVM profileViewVM)
    {
        m_ordnerFrameVM = ordnerFrameVM;
        m_nutzerFrameVM = nutzerFrameVM;
        m_sucheViewModel = sucheViewModel;
        m_dokumentenFrameVM = dokumentenFrameVM;
        m_profileVM = profileViewVM;

        OrdnerCommand = new DelegateCommand(OnOrdner);
        NutzerCommand = new DelegateCommand(OnNutzer);
        LogoutCommand = new DelegateCommand(OnLogout);
        SearchCommand = new DelegateCommand(OnSearch);
        ProfileCommand = new DelegateCommand(OnProfile);
        m_sucheViewModel.FolderOpened += OnFolderOpened;
        m_sucheViewModel.BenutzerOpened += OnBenutzerOpened;
        
    }

    public void Init(Benutzer currentUser)
    {
        CurrentUser = currentUser;
        CurrentView = m_ordnerFrameVM;
        m_ordnerFrameVM.Init(currentUser);
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
        LogoutEvent?.Invoke(this, EventArgs.Empty);
    }

    private void OnSearch(object? o)
    {
        if (!string.IsNullOrEmpty(SearchQuery))
        {
            m_sucheViewModel.Init(CurrentUser);
            m_sucheViewModel.SearchQuery = SearchQuery;
            m_sucheViewModel.SearchCommand.Execute(null);
            CurrentView = m_sucheViewModel;
        }
    }

    private void OnFolderOpened(object sender, Ordner folder)
    {
        m_dokumentenFrameVM.Init(folder, CurrentUser);
        CurrentView = m_dokumentenFrameVM;
    }
    private void OnBenutzerOpened(object sender, Benutzer benutzer)
    {
        m_nutzerFrameVM.Init(CurrentUser);
        CurrentView = m_nutzerFrameVM;     
    }

    private void OnProfile(object o)
    {
        m_profileVM.Init(CurrentUser);
        CurrentView = m_profileVM;
        m_profileVM.GoBack += OnGoback;
    }
    private void OnGoback(object sender, EventArgs e)
    {
        CurrentView = this;
        this.Init(CurrentUser);
    }
}
