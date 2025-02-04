﻿using DMS.ViewModel.Dokumentenuebersicht;
using DMS.ViewModel.Nutzerverwaltung;
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

namespace DMS.View.Nutzerverwaltung
{
    /// <summary>
    /// Interaktionslogik für NutzerView1.xaml
    /// </summary>
    public partial class NutzerView1 : UserControl
    {
        public NutzerView1()
        {
            InitializeComponent();
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is NutzerView1VM viewModel)
            {
                if (sender is StackPanel)
                {
                    if (viewModel.SaveBenutzerCommand.CanExecute(null))
                        viewModel.SaveBenutzerCommand.Execute(null);
                    viewModel.Init(viewModel.CurrentUser);
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
