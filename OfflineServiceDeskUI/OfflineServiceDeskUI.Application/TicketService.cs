using System;
using System.Collections.Generic;
using System.Text;

namespace OfflineServiceDeskUI.Application;

public sealed record TicketListItem(
    Guid Id, string Title, String status, DateTimeOffset UpdateAtUtc);

public interface ITicketService
{
    Task<IReadOnlyList<TicketListItem>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<TicketListItem> UpdateTitleAsync(
        Guid id, string? title, CancellationToken cancellationToken = default);
}

public sealed class UseCaseException(string message) : Exception(message);
