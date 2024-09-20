using System.Collections.ObjectModel;
using DMS.Model;
using DMS.Service;

namespace DMS.ViewModel.Nutzerverwaltung
{
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

        public async Task Init(Benutzer currentUser)
        {
            CurrentUser = currentUser;

            var users = await m_benutzerService.GetAllUsers();

            BenutzerCollection = new ObservableCollection<Benutzer>(
                users.OrderByDescending(b => b.IsAdmin)
                     .ThenByDescending(b => b.IsActive));
        }

        private void SaveChanges(object? o)
        {
            Benutzer benutzer = (Benutzer)o;
            if (benutzer != null)
            {
                m_benutzerService.SaveChanges(benutzer);
                Init(CurrentUser);
            }
        }
    }
}
