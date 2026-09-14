using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using OfflineServiceDesk.Application;
using OfflineServiceDesk.Core;

namespace OfflineServiceDesk.Desktop;

public sealed class MainViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
{
    private readonly ITicketService _service;
    private readonly Dictionary<string, List<string>> _errors = [];
    private TicketListItem? _selectedTicket;
    private string _editTitle = string.Empty;
    private bool _isBusy;
    private string? _message;

    public MainViewModel(ITicketService service)
    {
        _service = service;
        ReloadCommand = new AsyncRelayCommand(ReloadAsync, () => !IsBusy);
        SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
    }

    public ObservableCollection<TicketListItem> Tickets { get; } = [];
    public AsyncRelayCommand ReloadCommand { get; }
    public AsyncRelayCommand SaveCommand { get; }

    public TicketListItem? SelectedTicket
    {
        get => _selectedTicket;
        set
        {
            if (!SetField(ref _selectedTicket, value))
            {
                return;
            }

            // Selezione → copia modificabile nel form di dettaglio.
            EditTitle = value?.Title ?? string.Empty;
            SaveCommand.RaiseCanExecuteChanged();
        }
    }

    public string EditTitle
    {
        get => _editTitle;
        set
        {
            if (SetField(ref _editTitle, value))
            {
                ValidateTitle();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public bool IsBusy
    {
        get => _isBusy;
        private set
        {
            if (SetField(ref _isBusy, value))
            {
                OnPropertyChanged(nameof(BusyVisibility));
                ReloadCommand.RaiseCanExecuteChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public System.Windows.Visibility BusyVisibility =>
        IsBusy ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

    public string? Message
    {
        get => _message;
        private set => SetField(ref _message, value);
    }

    public bool HasErrors => _errors.Count != 0;
    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public IEnumerable GetErrors(string? propertyName) =>
        propertyName is not null && _errors.TryGetValue(propertyName, out List<string>? errors)
            ? errors
            : Array.Empty<string>();

    public Task InitializeAsync() => ReloadAsync();

    private async Task ReloadAsync()
    {
        await RunBusyAsync(async () =>
        {
            IReadOnlyList<TicketListItem> items = await _service.GetAllAsync();
            Tickets.Clear();

            foreach (TicketListItem item in items)
            {
                Tickets.Add(item);
            }

            SelectedTicket = Tickets.FirstOrDefault();
            Message = $"Caricati {Tickets.Count} ticket.";
        });
    }

    private async Task SaveAsync()
    {
        if (SelectedTicket is null)
        {
            return;
        }

        Guid selectedId = SelectedTicket.Id;

        await RunBusyAsync(async () =>
        {
            TicketListItem updated = await _service.UpdateTitleAsync(selectedId, EditTitle);
            int index = Tickets.IndexOf(SelectedTicket);
            Tickets[index] = updated;
            SelectedTicket = updated;
            Message = "Modifiche salvate.";
        });
    }

    private async Task RunBusyAsync(Func<Task> operation)
    {
        IsBusy = true;
        Message = null;

        try
        {
            await operation();
        }
        catch (DomainException exception)
        {
            Message = exception.Message;
        }
        catch (UseCaseException exception)
        {
            Message = exception.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool CanSave() =>
        !IsBusy && SelectedTicket is not null && !HasErrors;

    private void ValidateTitle()
    {
        _errors.Remove(nameof(EditTitle));

        if (string.IsNullOrWhiteSpace(EditTitle))
        {
            _errors[nameof(EditTitle)] = ["Il titolo è obbligatorio."];
        }
        else if (EditTitle.Trim().Length > 120)
        {
            _errors[nameof(EditTitle)] = ["Massimo 120 caratteri."];
        }

        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(EditTitle)));
        OnPropertyChanged(nameof(HasErrors));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(name);
        return true;
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}