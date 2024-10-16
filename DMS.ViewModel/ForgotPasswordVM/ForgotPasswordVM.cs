using DMS.Service;
using System.Windows.Input;
using ViewModel.Interface.ForgotPassword;
namespace DMS.ViewModel.ForgotPasswordVM
{
    public class ForgotPasswordVM : ViewModelBase, IForgotPasswordVM
    {
        private readonly BenutzerService _benutzerService;
        private string _benutzername;
        private string _email;
        private string _verificationCode;
        private string _newPassword;
        private string _confirmPassword;
        private string _errorMessage;
        private bool _isCodeSent;
        private bool _isCodeVerified;
        private readonly EmailService _emailService;

        public event EventHandler PasswordForgetWindow;
        public event EventHandler PasswordChangedEvent;

        public string Benutzername
        {
            get => _benutzername;
            set => SetField(ref _benutzername, value);
        }

        public string Email
        {
            get => _email;
            set => SetField(ref _email, value);
        }

        public string VerificationCode
        {
            get => _verificationCode;
            set => SetField(ref _verificationCode, value);
        }

        public string NewPassword
        {
            get => _newPassword;
            set => SetField(ref _newPassword, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetField(ref _confirmPassword, value);
        }

        public bool IsCodeSent
        {
            get => _isCodeSent;
            set => SetField(ref _isCodeSent, value);
        }

        public bool IsCodeVerified
        {
            get => _isCodeVerified;
            set => SetField(ref _isCodeVerified, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetField(ref _errorMessage, value);
        }

        public ICommand SendCodeCommand { get; }
        public ICommand VerifyCodeCommand { get; }
        public ICommand ChangePasswordCommand { get; }

        public ForgotPasswordVM(BenutzerService benutzerService, EmailService emailService)
        {
            _benutzerService = benutzerService;
            _emailService = emailService;
            SendCodeCommand = new DelegateCommand(OnSendCode);
            VerifyCodeCommand = new DelegateCommand(OnVerifyCode);
            ChangePasswordCommand = new DelegateCommand(OnChangePassword);
        }
        public void Init()
        {
        }

        private async void OnSendCode(object obj)
        {
            bool success = await _emailService.SendVerificationCode(Benutzername, Email);
            if (success)
            {
                IsCodeSent = true;
                ErrorMessage = "Bestätigungscode wurde gesendet.";
            }
            else
            {
                ErrorMessage = "Benutzername oder E-Mail ist nicht korrekt.";
            }
        }
        private void OnVerifyCode(object obj)
        {
            if (_emailService.VerifyCode(Benutzername, VerificationCode))
            {
                IsCodeVerified = true;
                ErrorMessage = "";
            }
            else
            {
                ErrorMessage = "Ungültiger Code.";
            }
        }

        private async void OnChangePassword(object obj)
        {
            if (NewPassword == ConfirmPassword)
            {
                bool success = await _benutzerService.ChangePassword(Benutzername, NewPassword);
                if (success)
                {
                    ErrorMessage = "Passwort erfolgreich geändert.";
                    PasswordChangedEvent?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    ErrorMessage = "Fehler beim Ändern des Passworts.";
                }
            }
            else
            {
                ErrorMessage = "Passwörter stimmen nicht überein.";
            }
        }
    }
}