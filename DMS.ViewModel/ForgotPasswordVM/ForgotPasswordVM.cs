using DMS.Service;
using System.Windows.Input;
using ViewModel.Interface.ForgotPassword;

namespace DMS.ViewModel.ForgotPasswordVM
{
    public class ForgotPasswordVM : ViewModelBase, IForgotPasswordVM
    {
        private readonly BenutzerService _benutzerService;
        private readonly EmailService _emailService;
        private string _benutzername;
        private string _email;
        private string _verificationCode;
        private string _newPassword;
        private string _confirmPassword;
        private string _message;
        private bool _isCodeSent;
        private bool _isCodeVerified;

        private bool _isUsernameErrorVisible;
        private bool _isEmailErrorVisible;
        private bool _isVerificationCodeErrorVisible;
        private bool _isNewPasswordErrorVisible;
        private bool _isConfirmPasswordErrorVisible;
        private bool _passwordsDoNotMatch;

        public event EventHandler PasswordForgetWindow;
        public event EventHandler PasswordChangedEvent;
        public event EventHandler GoBackEvent;

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

        public string Message
        {
            get => _message;
            set => SetField(ref _message, value);
        }

        public bool IsUsernameErrorVisible
        {
            get => _isUsernameErrorVisible;
            set => SetField(ref _isUsernameErrorVisible, value);
        }

        public bool IsEmailErrorVisible
        {
            get => _isEmailErrorVisible;
            set => SetField(ref _isEmailErrorVisible, value);
        }

        public bool IsVerificationCodeErrorVisible
        {
            get => _isVerificationCodeErrorVisible;
            set => SetField(ref _isVerificationCodeErrorVisible, value);
        }

        public bool IsNewPasswordErrorVisible
        {
            get => _isNewPasswordErrorVisible;
            set => SetField(ref _isNewPasswordErrorVisible, value);
        }

        public bool IsConfirmPasswordErrorVisible
        {
            get => _isConfirmPasswordErrorVisible;
            set => SetField(ref _isConfirmPasswordErrorVisible, value);
        }

        public bool PasswordsDoNotMatch
        {
            get => _passwordsDoNotMatch;
            set => SetField(ref _passwordsDoNotMatch, value);
        }

        public ICommand GoBackCommand { get; set; }
        public DelegateCommand SendCodeCommand { get; }
        public DelegateCommand VerifyCodeCommand { get; }
        public DelegateCommand ChangePasswordCommand { get; }

        public ForgotPasswordVM(BenutzerService benutzerService, EmailService emailService)
        {
            _benutzerService = benutzerService;
            _emailService = emailService;
            SendCodeCommand = new DelegateCommand(OnSendCode);
            VerifyCodeCommand = new DelegateCommand(OnVerifyCode);
            ChangePasswordCommand = new DelegateCommand(OnChangePassword);
            GoBackCommand = new DelegateCommand(OnGoBack);
            Init();
        }

        private async void OnSendCode(object obj)
        {
            IsUsernameErrorVisible = string.IsNullOrEmpty(Benutzername);
            IsEmailErrorVisible = string.IsNullOrEmpty(Email);

            if (IsUsernameErrorVisible || IsEmailErrorVisible)
                return;

            if (!await _benutzerService.UserExists(Benutzername))
            {
                Message = "Benutzername oder E-Mail ist nicht korrekt.";
                return;
            }
            bool success = await _emailService.SendVerificationCode(Benutzername, Email);
            IsCodeSent = success;
            Message = success ? "Bestätigungscode wurde gesendet." : "Fehler beim Senden des Bestätigungscodes.";

        }

        private void OnVerifyCode(object obj)
        {

            IsVerificationCodeErrorVisible = string.IsNullOrEmpty(VerificationCode);
            if (IsVerificationCodeErrorVisible)
                return;

            IsCodeVerified = _emailService.VerifyCode(Benutzername, VerificationCode);
            Message = IsCodeVerified ? "Bestätigt." : "Ungültiger Code.";
        }

        private async void OnChangePassword(object obj)
        {
            IsNewPasswordErrorVisible = string.IsNullOrEmpty(NewPassword);
            IsConfirmPasswordErrorVisible = string.IsNullOrEmpty(ConfirmPassword);
            PasswordsDoNotMatch = NewPassword != ConfirmPassword;

            if (IsNewPasswordErrorVisible || IsConfirmPasswordErrorVisible || PasswordsDoNotMatch)
                return;

            bool success = await _benutzerService.ChangePassword(Benutzername, NewPassword);
            if (success)
            {
                Message = "Passwort erfolgreich geändert.";
                PasswordChangedEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnGoBack(object obj)
        {
            GoBackEvent?.Invoke(this, EventArgs.Empty);
        }
        public void Init()
        {
            _benutzername = string.Empty;
            _email = string.Empty;
            _verificationCode = string.Empty;
            _newPassword = string.Empty;
            _confirmPassword = string.Empty;
            _message = string.Empty;
            _isCodeSent = false;
            _isCodeVerified = false;
            _isUsernameErrorVisible = false;
            _isEmailErrorVisible = false;
            _isVerificationCodeErrorVisible = false;
            _isNewPasswordErrorVisible = false;
            _isConfirmPasswordErrorVisible = false;
            _passwordsDoNotMatch = false;

        }
    }
}
