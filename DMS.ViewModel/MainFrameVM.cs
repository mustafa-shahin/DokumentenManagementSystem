using DMS.Model;
using DMS.ViewModel.Dokumentenuebersicht;
using DMS.ViewModel.Nutzerverwaltung;
using DMS.ViewModel.Ordneruebersicht;
using System.Windows.Input;
using ViewModel.Interface;
using ViewModel.Interface.Suche;

namespace DMS.ViewModel;

public class MainFrameVM : ViewModelBase, IMainFrameVM
{
    private Benutzer m_currentUser;
    private IOrdnerFrameVM m_ordnerFrameVM;
    private INutzerFrameVM m_nutzerFrameVM;
    private ISucheViewModel m_sucheViewModel;
    private IDokumentenFrameVM m_dokumentenFrameVM;
    private IViewModelBase m_currentView;
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

    public MainFrameVM(IOrdnerFrameVM ordnerFrameVM, INutzerFrameVM nutzerFrameVM, ISucheViewModel sucheViewModel, IDokumentenFrameVM dokumentenFrameVM)
    {
        m_ordnerFrameVM = ordnerFrameVM;
        m_nutzerFrameVM = nutzerFrameVM;
        m_sucheViewModel = sucheViewModel;
        m_dokumentenFrameVM = dokumentenFrameVM;
        OrdnerCommand = new DelegateCommand(OnOrdner);
        NutzerCommand = new DelegateCommand(OnNutzer);
        LogoutCommand = new DelegateCommand(OnLogout);
        SearchCommand = new DelegateCommand(OnSearch);

        m_sucheViewModel.FolderOpened += OnFolderOpened;
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
}
