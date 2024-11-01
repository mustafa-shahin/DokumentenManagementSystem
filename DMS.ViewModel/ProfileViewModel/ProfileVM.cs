using DMS.Model;
using DMS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModel.Interface.Profile;

namespace DMS.ViewModel.ProfileVM
{
    public class ProfileVM : ViewModelBase, IProfileViewVM
    {

        private readonly BenutzerService _benutzerService;
        private Benutzer _benutzer;
        private string _benutzername;
        private string _newPassword;
        private string _confirmPassword;
        private string _currentPassword;
        public event EventHandler GoBack;
        private bool _passwordsDoNotMatch;
        private bool _wrongPassword;
        private bool _passwordchanged;
        public string Benutzername
        {
            get => _benutzer.Name;
            set => SetField(ref _benutzername, value);
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
        public string CurrentPassword
        {
            get => _confirmPassword;
            set => SetField(ref _currentPassword, value);
        }
        public bool PasswordsDoNotMatch
        {
            get => _passwordsDoNotMatch;
            set => SetField(ref _passwordsDoNotMatch, value);
        }
        public bool WrongPassword
        {
            get => _wrongPassword;
            set => SetField(ref _wrongPassword, value);
        }
        public bool PasswordChanged
        {
            get => _passwordchanged;
            set => SetField(ref _passwordchanged, value);
        }


        public DelegateCommand ChangePasswordCommand { get; }
        public DelegateCommand GoBackCommand { get; set; }
        public ProfileVM(BenutzerService benutzerService, Benutzer benutzer) 
        { 
            _benutzerService = benutzerService;
            _benutzer = benutzer;
            ChangePasswordCommand = new DelegateCommand(OnChangePassword);
            GoBackCommand = new DelegateCommand(OnGoBack);


        }

        private async void OnChangePassword(object obj)
        {
            PasswordsDoNotMatch = NewPassword != ConfirmPassword;
            if (NewPassword != ConfirmPassword)
                return;

            var success = await _benutzerService.VerifyAndUpdatePassword(_benutzer.Id, NewPassword);
            WrongPassword = !success;
            PasswordChanged = success;
        }
        private void OnGoBack(object obj)
        {
            GoBack?.Invoke(this, EventArgs.Empty);
        }
        public void Init(Benutzer benutzer)
        {
            _benutzer = benutzer;
            WrongPassword = false;
            PasswordChanged = false;
        }
    }
}
