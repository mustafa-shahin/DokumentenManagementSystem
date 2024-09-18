using ViewModel.Interface.Login;

namespace DMS.ViewModel.Login;

public class LoginVM : ViewModelBase, ILoginVM
{
    private string m_benutzer;
    private string m_passwort;

    public string Benutzer
    {
        get => m_benutzer;
        set
        {
            if (value == m_benutzer) return;
            m_benutzer = value;
            OnPropertyChanged();
        }
    }
    
    public string Passwort
    {
        get => m_passwort;
        set
        {
            if (value == m_passwort) return;
            m_passwort = value;
            OnPropertyChanged();
        }
    }
}