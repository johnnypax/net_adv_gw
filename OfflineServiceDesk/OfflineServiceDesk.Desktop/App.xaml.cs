using System.Windows;
using OfflineServiceDesk.Application;
using OfflineServiceDesk.Infrastructure;

namespace OfflineServiceDesk.Desktop;

public partial class App : System.Windows.Application
{
    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Composition Root della demo: la ViewModel riceve un contratto Application.
        ITicketService service = new InMemoryTicketService();
        var viewModel = new MainViewModel(service);
        var window = new MainWindow(viewModel);

        MainWindow = window;
        window.Show();
        await viewModel.InitializeAsync();
    }
}