using DMS.Model;

namespace DMS.ViewModel.Ordneruebersicht;

public class OrdnerView1VM : ViewModelBase, IOrdnerView1VM
{
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

    public void Init(Benutzer currentUser)
    {
        CurrentUser = currentUser;
    }
}