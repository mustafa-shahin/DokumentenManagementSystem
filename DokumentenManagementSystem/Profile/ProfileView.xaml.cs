using DMS.ViewModel.ForgotPasswordVM;
using DMS.ViewModel.Login;
using DMS.ViewModel.ProfileVM;
using DokumentenManagementSystem.UI_Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DokumentenManagementSystem.Profile
{
    /// <summary>
    /// Interaction logic for ProfileView.xaml
    /// </summary>
    public partial class ProfileView : UserControl
    {
        public ProfileView()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProfileVM profileVM)
            {
                if (sender is PasswordBox passwordBox)
                {
                    profileVM.CurrentPassword = passwordBox.Password;
                }
            }
        }
        private void New_PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProfileVM profileVM)
            {
                if (sender is PasswordBox passwordBox)
                {
                    profileVM.NewPassword = passwordBox.Password;
                }
            }
        }
        private void OnConfirmPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProfileVM profileVM)
            {
                if (sender is PasswordBox passwordBox)
                {
                    profileVM.ConfirmPassword = passwordBox.Password;
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
