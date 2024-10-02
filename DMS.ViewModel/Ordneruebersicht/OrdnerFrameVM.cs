using DMS.Model;
using DMS.Service;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ViewModel.Interface;

namespace DMS.ViewModel.Ordneruebersicht
{
    public class OrdnerFrameVM : ViewModelBase, IOrdnerFrameVM
    {
        private Benutzer m_currentUser;
        private readonly IOrdnerView1VM m_ordnerView1VM;
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

        public OrdnerFrameVM(IOrdnerView1VM ordnerview1)
        {
            m_ordnerView1VM = ordnerview1;
        }

        public void Init(Benutzer benutzer)
        {
            m_currentUser = benutzer;
            m_ordnerView1VM.Init(m_currentUser);
            CurrentView = m_ordnerView1VM;
        }
        
    }
}
