﻿using DMS.DataAccess;
using DMS.Service;
using DMS.ViewModel.Dokumentenuebersicht;
using DMS.ViewModel.Nutzerverwaltung;
using DMS.ViewModel.Ordneruebersicht;
using DMS.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;
using DMS.ViewModel.Login;
using Unity;
using ViewModel.Interface;
using ViewModel.Interface.Login;
using DMS.ViewModel.ForgotPasswordVM;
using ViewModel.Interface.ForgotPassword;
using ViewModel.Interface.Suche;
using DMS.ViewModel.SucheViewModel;
using Microsoft.EntityFrameworkCore;
using DMS.ViewModel.ProfileVM;
using System;
using ViewModel.Interface.Profile;

namespace DokumentenManagementSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private UnityContainer m_container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            m_container = new UnityContainer();

            // Dynamischer Pfad für die SQLite-Datenbank
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = System.IO.Path.Join(Environment.GetFolderPath(folder), "dms.db");

            // Erstelle die DbContextOptions explizit
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlite($"Data Source={path}"); // Dynamischer SQLite-Datenbankpfad

            // DbContextOptions und DataContext registrieren
            m_container.RegisterInstance(optionsBuilder.Options); // Registriere die Optionen im Container
            m_container.RegisterType<DataContext>(); // Registriere den DataContext im Container

            // Registriere deine ViewModels und Services
            m_container.RegisterType<MainWindowViewModel>();
            m_container.RegisterType<IMainFrameVM, MainFrameVM>();
            m_container.RegisterType<IDokumentenFrameVM, DokumentenFrameVM>();
            m_container.RegisterType<IDokumentenView1VM, DokumentenView1VM>();
            m_container.RegisterType<INutzerFrameVM, NutzerFrameVM>();
            m_container.RegisterType<INutzerView1VM, NutzerView1VM>();
            m_container.RegisterType<IOrdnerFrameVM, OrdnerFrameVM>();
            m_container.RegisterType<IOrdnerView1VM, OrdnerView1VM>();
            m_container.RegisterType<ILoginVM, LoginVM>();
            m_container.RegisterType<IForgotPasswordVM, ForgotPasswordVM>();
            m_container.RegisterType<IProfileViewVM, ProfileVM>();
            m_container.RegisterType<ISucheViewVM, SucheViewModel>();
            m_container.RegisterType<BenutzerService>();
            m_container.RegisterType<DokumenteService>();
            m_container.RegisterType<OrdnerService>();
            m_container.RegisterType<EmailService>();

            // Hauptfenster anzeigen
            MainWindowViewModel main = m_container.Resolve<MainWindowViewModel>();
            MainWindow mainWindow = new()
            {
                DataContext = main
            };
            mainWindow.Show();
        }
    }
}