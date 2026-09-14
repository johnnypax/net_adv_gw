using System.Windows;

namespace OfflineServiceDesk.Desktop;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();

        // Il code-behind collega la ViewModel, ma non contiene query o regole di dominio.
        DataContext = viewModel;
    }
}