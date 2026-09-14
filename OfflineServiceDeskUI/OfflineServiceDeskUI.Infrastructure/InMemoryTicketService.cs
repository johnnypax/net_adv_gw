using OfflineServiceDeskUI.Application;
using OfflineServiceDeskUI.Core;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace OfflineServiceDeskUI.Infrastructure;

public sealed class InMemoryTicketService : ITicketService
{
    private readonly List<Ticket> _tickets =
    [
        new(Guid.Parse("11111111-1111-1111-1111-111111111111"),
            "Stampante reception bloccata", "Aperto", DateTimeOffset.UtcNow.AddMinutes(-30)),
        new(Guid.Parse("22222222-2222-2222-2222-222222222222"),
            "VPN sede distaccata", "In lavorazione", DateTimeOffset.UtcNow.AddMinutes(-12)),
        new(Guid.Parse("33333333-3333-3333-3333-333333333333"),
            "Aggiornamento inventario", "Chiuso", DateTimeOffset.UtcNow.AddHours(-2))
    ];

    public async Task<IReadOnlyList<TicketListItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await Task.Delay(500, cancellationToken);

        return _tickets.OrderByDescending(ticket => ticket.UpdatedAtUtc).Select(ToListItem).ToArray();
    }

    public async Task<TicketListItem> UpdateTitleAsync(Guid id, string? title, CancellationToken cancellationToken = default)
    {
        await Task.Delay(500, cancellationToken);

        Ticket ticket = _tickets.SingleOrDefault(ticket => ticket.Id == id) ?? 
            throw new UseCaseException("Ticket not found");

        ticket.Rename(title, DateTimeOffset.UtcNow);
        return ToListItem(ticket);
    }

    private static TicketListItem ToListItem(Ticket ticket) =>
        new(ticket.Id, ticket.Title, ticket.Status, ticket.UpdatedAtUtc);
}

