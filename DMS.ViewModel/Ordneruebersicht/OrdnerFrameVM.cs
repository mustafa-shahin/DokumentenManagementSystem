using DMS.Model;
using DMS.ViewModel.Dokumentenuebersicht;
using ViewModel.Interface;

namespace DMS.ViewModel.Ordneruebersicht
{
    public class OrdnerFrameVM : ViewModelBase, IOrdnerFrameVM
    {
        private Benutzer m_currentUser;
        private readonly IOrdnerView1VM m_ordnerView1VM;
        private IViewModelBase m_currentView;
        private readonly IDokumentenFrameVM m_dokumentenFrameVM;
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

        public OrdnerFrameVM(IOrdnerView1VM ordnerView1VM, IDokumentenFrameVM dokumentenFrameVM)
        {
            m_ordnerView1VM = ordnerView1VM;
            m_dokumentenFrameVM = dokumentenFrameVM;

            m_ordnerView1VM.FolderOpened += OnFolderOpened;
        }

        public void Init(Benutzer benutzer)
        {
            m_currentUser = benutzer;
            m_ordnerView1VM.Init(m_currentUser);
            CurrentView = m_ordnerView1VM;
        }
        private void OnFolderOpened(object sender, Ordner folder)
        {
            m_dokumentenFrameVM.Init(folder, m_currentUser);
            CurrentView = m_dokumentenFrameVM;
        }
    }
}
