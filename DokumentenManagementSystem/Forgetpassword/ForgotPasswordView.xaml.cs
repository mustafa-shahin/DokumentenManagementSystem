using System.Windows.Controls;
using System.Windows;
using DMS.ViewModel.ForgotPasswordVM;

namespace DokumentenManagementSystem
{
    public partial class ForgotPasswordView : UserControl
    {
        public ForgotPasswordView()
        {
            InitializeComponent();
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ForgotPasswordVM viewModel)
            {
                var passwordBox = sender as PasswordBox;
                if (passwordBox != null)
                {
                    viewModel.NewPassword = passwordBox.Password;
                }
            }
        }

        private void OnConfirmPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ForgotPasswordVM viewModel)
            {
                var passwordBox = sender as PasswordBox;
                if (passwordBox != null)
                {
                    viewModel.ConfirmPassword = passwordBox.Password;
                }
            }
        }
    }
}
