using System.Collections.ObjectModel;
using DMS.Model;
using DMS.Service;

namespace DMS.ViewModel.Nutzerverwaltung;

public class NutzerView1VM : ViewModelBase, INutzerView1VM
{
    private BenutzerService m_benutzerService;

    private Benutzer m_currentUser;
    private ObservableCollection<Benutzer> m_benutzerCollection;

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

    public ObservableCollection<Benutzer> BenutzerCollection
    {
        get => m_benutzerCollection;
        set
        {
            if (Equals(value, m_benutzerCollection)) return;
            m_benutzerCollection = value;
            OnPropertyChanged();
        }
    }

    public DelegateCommand SaveBenutzerCommand { get; set; }

    public NutzerView1VM(BenutzerService benutzerService)
    {
        m_benutzerService = benutzerService;

        SaveBenutzerCommand = new DelegateCommand(SaveChanges);
    }

    public void Init(Benutzer currentUser)
    {
        CurrentUser = currentUser;
        BenutzerCollection = new ObservableCollection<Benutzer>(m_benutzerService.GetAllUsers());
    }

    private void SaveChanges(object? o)
    {
        Benutzer benutzer = (Benutzer)o;
        if(benutzer != null)
        {
            m_benutzerService.SaveChanges(benutzer);
            Init(CurrentUser);
        }
    }
}