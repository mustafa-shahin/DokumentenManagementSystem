using DMS.Model;
using ViewModel.Interface;

namespace DMS.ViewModel;

public class MainFrameVM : ViewModelBase, IMainFrameVM
{
    private Benutzer m_benutzer;

    public Benutzer Benutzer
    {
        get => m_benutzer;
        set
        {
            if (Equals(value, m_benutzer)) return;
            m_benutzer = value;
            OnPropertyChanged();
        }
    }

    public DelegateCommand LogoutCommand { get; set; }

    public event EventHandler? LogoutEvent;

    public MainFrameVM()
    {

    }

    public void Init(Benutzer benutzer)
    {
        Benutzer = benutzer;

        LogoutCommand = new DelegateCommand(OnLogout);
    }

    private void OnLogout(object? o)
    {
        LogoutEvent.Invoke(this, EventArgs.Empty);
    }
}