
using DMS.Model;
using ViewModel.Interface;
using DMS.ViewModel.Dokumentenuebersicht;

namespace DMS.ViewModel.Dokumentenuebersicht
{
    public class DokumentenFrameVM : ViewModelBase, IDokumentenFrameVM
    {
        private Ordner m_currentFolder;
        private Benutzer m_currentUser;
        private readonly IDokumentenView1VM m_dokumentenView1VM;
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

        public DokumentenFrameVM(IDokumentenView1VM dokumentenView1VM)
        {
            m_dokumentenView1VM = dokumentenView1VM;
        }

        public void Init(Ordner folder, Benutzer currentUser)
        {
            m_currentFolder = folder;
            m_currentUser = currentUser;

            m_dokumentenView1VM.Init(m_currentFolder, m_currentUser);
            CurrentView = m_dokumentenView1VM;
        }
    }
}
