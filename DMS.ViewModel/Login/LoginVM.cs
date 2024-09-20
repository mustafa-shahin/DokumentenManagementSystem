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
        private string m_loginErrorMessage;
        private string m_signupErrorMessage;

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

        public string LoginErrorMessage
        {
            get => m_loginErrorMessage;
            set
            {
                m_loginErrorMessage = value;
                OnPropertyChanged();
            }
        }

        public string SignupErrorMessage
        {
            get => m_signupErrorMessage;
            set
            {
                m_signupErrorMessage = value;
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
            LoginErrorMessage = string.Empty;
            SignupErrorMessage = string.Empty;
        }

        private bool CanExecuteLogin()
        {
            return !string.IsNullOrEmpty(Benutzer) && !string.IsNullOrEmpty(Passwort);
        }

        private void ExecuteLogin(object? parameter)
        {
            if (CanExecuteLogin())
            {
                var currentUser = m_benutzerService.LoginUser(Benutzer, Passwort);
                if (currentUser != null)
                {
                    LoginErrorMessage = string.Empty;
                    LoginEvent?.Invoke(this, currentUser);
                }
                else
                {
                    LoginErrorMessage = "Benutzername oder Passwort ist falsch.";
                }
            }
        }

        private async void ExecuteSignup(object parameter)
        {
            if (SignupPasswort != SignupPasswortConfirm)
            {
                SignupErrorMessage = "Die eingegebenen Passwörter sind nicht identisch.";
                return;
            }

            if (await m_benutzerService.UserExist(SignupBenutzer))
            {
                SignupErrorMessage = "Der Benutzername ist bereits vergeben.";
                return;
            }

            try
            {
                var newUser = new Benutzer
                {
                    Name = SignupBenutzer,
                    Passwort = SignupPasswort
                };


                bool success = m_benutzerService.CreateUser(SignupBenutzer, SignupPasswort);
                if (success)
                {
                    SignupErrorMessage = string.Empty;
                    SignupBenutzer = string.Empty;
                    SignupPasswort = string.Empty;
                    SignupPasswortConfirm = string.Empty;
                }
            }
            catch (Exception ex)
            {
                SignupErrorMessage = $"Ein Fehler ist aufgetreten: {ex.Message}";
            }
        }
    }
}
