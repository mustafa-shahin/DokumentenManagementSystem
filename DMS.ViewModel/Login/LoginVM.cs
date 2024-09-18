using DMS.Service;
using System.Windows.Input;
using ViewModel.Interface.Login;
using System.Windows;
using DMS.ViewModel;
using System.Diagnostics; // For RelayCommand

namespace DMS.ViewModel.Login
{
    public class LoginVM : ViewModelBase, ILoginVM
    {
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
                if (SetField(ref m_benutzer, value))
                {
                    //könnte für loggin gebraucht werden 
                }
            }
        }

        public string Passwort
        {
            get => m_passwort;
            set
            {
                if (SetField(ref m_passwort, value))
                {
                    // könnte für loggin gebraucht werden 
                }
            }
        }

        public string SignupBenutzer
        {
            get => m_signupBenutzer;
            set
            {
                if (SetField(ref m_signupBenutzer, value))
                {
                    ((RelayCommand)SignupCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string SignupPasswort
        {
            get => m_signupPasswort;
            set
            {
                if (SetField(ref m_signupPasswort, value))
                {
                    ((RelayCommand)SignupCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string SignupPasswortConfirm
        {
            get => m_signupPasswortConfirm;
            set
            {
                if (SetField(ref m_signupPasswortConfirm, value))
                {
                    ((RelayCommand)SignupCommand).RaiseCanExecuteChanged();
                }
            }
        }

        private ICommand m_signupCommand;
        public ICommand SignupCommand
        {
            get
            {
                if (m_signupCommand == null)
                {
                    m_signupCommand = new RelayCommand(ExecuteSignup, CanExecuteSignup);
                }
                return m_signupCommand;
            }
        }

        private bool CanExecuteSignup(object parameter)
        {
            bool canExecute = !string.IsNullOrEmpty(SignupBenutzer) &&
                              !string.IsNullOrEmpty(SignupPasswort) &&
                              SignupPasswort == SignupPasswortConfirm;
            return canExecute;
        }


        private void ExecuteSignup(object parameter)
        {
            var userService = new BenutzerService();
            bool success = userService.CreateUser(SignupBenutzer, SignupPasswort);
            if (success)
            {
                // Optionally clear the fields
                SignupBenutzer = string.Empty;
                SignupPasswort = string.Empty;
                SignupPasswortConfirm = string.Empty;
            }
            else
            {
            }
        }

        // Optional: Implement LoginCommand similarly if needed
        private ICommand m_loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                if (m_loginCommand == null)
                {
                    m_loginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
                }
                return m_loginCommand;
            }
        }

        private bool CanExecuteLogin(object parameter)
        {
            return !string.IsNullOrEmpty(Benutzer) && !string.IsNullOrEmpty(Passwort);
        }

        private void ExecuteLogin(object parameter)
        {
            var userService = new BenutzerService();
            bool isValidUser = userService.ValidateUser(Benutzer, Passwort);
            if (isValidUser)
            {

            }
            else
            {
              
            }
        }
    }
}
