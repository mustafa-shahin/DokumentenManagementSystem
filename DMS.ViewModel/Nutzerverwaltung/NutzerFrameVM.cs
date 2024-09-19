using DMS.Model;
using DMS.ViewModel.Ordneruebersicht;
using ViewModel.Interface;

namespace DMS.ViewModel.Nutzerverwaltung;

public class NutzerFrameVM : ViewModelBase, INutzerFrameVM
{
    private INutzerView1VM m_nutzerView1VM;

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

    public NutzerFrameVM(INutzerView1VM nutzerView1VM)
    {
        m_nutzerView1VM = nutzerView1VM;
    }

    public void Init(Benutzer currentUser)
    {
        m_nutzerView1VM.Init(currentUser);
        CurrentView = m_nutzerView1VM;
    }
}