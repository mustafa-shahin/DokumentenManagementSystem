using DMS.DataAccess;
using DMS.Service;
using DMS.ViewModel.Dokumentenuebersicht;
using DMS.ViewModel.Nutzerverwaltung;
using DMS.ViewModel.Ordneruebersicht;
using DMS.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;
using Unity;

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
            m_container.RegisterType<DokumentenFrameVM>();
            m_container.RegisterType<DokumentenView1VM>();
            m_container.RegisterType<NutzerFrameVM>();
            m_container.RegisterType<NutzerView1VM>();
            m_container.RegisterType<OrdnerFrameVM>();
            m_container.RegisterType<OrdnerView1VM>();
            m_container.RegisterType<BenutzerService>();
            m_container.RegisterType<DokumenteService>();
            m_container.RegisterType<OrdnerService>();
            m_container.RegisterType<DataAccess>();

            MainWindowViewModel main = m_container.Resolve<MainWindowViewModel>();

            MainWindow mainWindow = new MainWindow()
            {
                DataContext = main
            };
            mainWindow.Show();
        }
    }

}
