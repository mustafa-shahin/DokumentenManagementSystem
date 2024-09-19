using DMS.DataAccess;
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
            m_container.RegisterType<MainWindowViewModel>();
            m_container.RegisterType<IMainFrameVM, MainFrameVM>();
            m_container.RegisterType<IDokumentenFrameVM, DokumentenFrameVM>();
            m_container.RegisterType<IDokumentenView1VM, DokumentenView1VM>();
            m_container.RegisterType<INutzerFrameVM, NutzerFrameVM>();
            m_container.RegisterType<INutzerView1VM, NutzerView1VM>();
            m_container.RegisterType<IOrdnerFrameVM, OrdnerFrameVM>();
            m_container.RegisterType<IOrdnerView1VM, OrdnerView1VM>();
            m_container.RegisterType<ILoginVM, LoginVM>();

            m_container.RegisterType<BenutzerService>();
            m_container.RegisterType<DokumenteService>();
            m_container.RegisterType<OrdnerService>();
            MainWindowViewModel main = m_container.Resolve<MainWindowViewModel>();

            MainWindow mainWindow = new MainWindow()
            {
                DataContext = main
            };
            mainWindow.Show();
           
        }
    }

}
