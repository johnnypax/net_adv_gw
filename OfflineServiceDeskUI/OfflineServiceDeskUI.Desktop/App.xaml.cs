using OfflineServiceDesk.Desktop;
using OfflineServiceDeskUI.Application;
using OfflineServiceDeskUI.Infrastructure;
using System.Configuration;
using System.Data;
using System.Windows;

namespace OfflineServiceDeskUI.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //Startup point -> Composition Root
            ITicketService service = new InMemoryTicketService();
            var viewModel = new MainViewModel(service);
            var window = new MainWindow(viewModel);

            MainWindow = window;
            window.Show();
            //TODO: Inizializzazione del View Model
        }
    }

}
