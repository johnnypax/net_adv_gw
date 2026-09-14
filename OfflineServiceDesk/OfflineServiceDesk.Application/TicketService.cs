namespace OfflineServiceDesk.Application;

// DTO pensato per il binding: la View non riceve l'entità di dominio.
public sealed record TicketListItem(
    Guid Id,
    string Title,
    string Status,
    DateTimeOffset UpdatedAtUtc);

public interface ITicketService
{
    Task<IReadOnlyList<TicketListItem>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<TicketListItem> UpdateTitleAsync(
        Guid id,
        string? title,
        CancellationToken cancellationToken = default);
}

public sealed class UseCaseException(string message) : Exception(message);