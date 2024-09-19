using DMS.Model;
using DMS.Service;
using ViewModel.Interface.Login;

namespace DMS.ViewModel.Login
{
    public class LoginVM : ViewModelBase, ILoginVM
    {
        private BenutzerService m_benutzerService;

        private string m_benutzer;
        private string m_passwort;
        private string m_signupBenutzer;
        private string m_signupPasswort;
        private string m_signupPasswortConfirm;

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

        public string SignupBenutzer
        {
            get => m_signupBenutzer;
            set
            {
                if (value == m_signupBenutzer) return;
                m_signupBenutzer = value;
                OnPropertyChanged();
            }
        }

        public string SignupPasswort
        {
            get => m_signupPasswort;
            set
            {
                if (value == m_signupPasswort) return;
                m_signupPasswort = value;
                OnPropertyChanged();
            }
        }

        public string SignupPasswortConfirm
        {
            get => m_signupPasswortConfirm;
            set
            {
                if (value == m_signupPasswortConfirm) return;
                m_signupPasswortConfirm = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand SignUpCommand { get; set; }

        public event EventHandler<Benutzer> LoginEvent;

        public LoginVM(BenutzerService benutzerService)
        {
            m_benutzerService = benutzerService;

            LoginCommand = new DelegateCommand(ExecuteLogin);
            SignUpCommand = new DelegateCommand(ExecuteSignup);

            Init();
        }

        public void Init()
        {
            Benutzer = string.Empty;
            Passwort = string.Empty;
            SignupBenutzer = string.Empty;
            SignupPasswort = string.Empty;
            SignupPasswortConfirm = string.Empty;
        }

        private bool CanExecuteLogin()
        {
            return !string.IsNullOrEmpty(Benutzer) && !string.IsNullOrEmpty(Passwort);
        }

        private void ExecuteLogin(object? parameter)
        {
            if (CanExecuteLogin())
            {
                Benutzer currentUser = m_benutzerService.LoginUser(Benutzer, Passwort);
                if (currentUser != null)
                {
                    LoginEvent.Invoke(this, currentUser);
                }
                else
                {

                }
            }
        }

        private bool CanExecuteSignup()
        {
            bool canExecute = !string.IsNullOrEmpty(SignupBenutzer) &&
                              !string.IsNullOrEmpty(SignupPasswort) &&
                              SignupPasswort == SignupPasswortConfirm;
            return canExecute;
        }


        private void ExecuteSignup(object parameter)
        {
            if (CanExecuteSignup())
            {
                bool success = m_benutzerService.CreateUser(SignupBenutzer, SignupPasswort);
                if (success)
                {
                    SignupBenutzer = string.Empty;
                    SignupPasswort = string.Empty;
                    SignupPasswortConfirm = string.Empty;
                }
                else
                {
                }
            }
        }
    }
}
