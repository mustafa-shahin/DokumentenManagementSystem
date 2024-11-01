using System.Windows.Controls;
using System.Windows;
using DMS.ViewModel.ForgotPasswordVM;
using DokumentenManagementSystem.UI_Behavior;
using System.Windows.Input;

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
                if (sender is PasswordBox passwordBox)
                {
                    viewModel.NewPassword = passwordBox.Password;
                }
            }
        }

        private void OnConfirmPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ForgotPasswordVM viewModel)
            {
                if (sender is PasswordBox passwordBox)
                {
                    viewModel.ConfirmPassword = passwordBox.Password;
                }
            }
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            HoverEffect.ResizeOnHover(sender, e);
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            HoverEffect.ResizeOnHover(sender, e);
        }
    }
}
