using DMS.ViewModel.Login;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DokumentenManagementSystem.Login
{
    /// <summary>
    /// Interaktionslogik für LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginVM loginVM)
            {
                if (sender is PasswordBox passwordBox)
                {
                    loginVM.Passwort = passwordBox.Password;
                }
            }
        }

        private void PasswordBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (DataContext is LoginVM loginVM && sender is PasswordBox passwordBox)
                {
                    loginVM.LoginCommand.Execute(null);
                }
            }
        }
    }
}
