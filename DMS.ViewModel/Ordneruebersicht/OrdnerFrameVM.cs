using DMS.Model;
using ViewModel.Interface;

namespace DMS.ViewModel.Ordneruebersicht;

public class OrdnerFrameVM : ViewModelBase, IOrdnerFrameVM
{
    private IOrdnerView1VM m_ordnerView1VM;

    private IViewModelBase m_currentView;

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

    public OrdnerFrameVM(IOrdnerView1VM ordnerView1VM)
    {
        m_ordnerView1VM = ordnerView1VM;
    }

    public void Init(Benutzer currentUser)
    {
        m_ordnerView1VM.Init(currentUser);
        CurrentView = m_ordnerView1VM;
    }
}