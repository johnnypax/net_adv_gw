using OfficeServiceDesk.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OfficeServiceDesk.Application;
public sealed record OpenTicketCommand(string? Title, string Priority);

public sealed record TicketDto(Guid id, string title, string priority);

//Contratto per Infrastructure!
public interface ITicketRepository
{
    Task AddAsync(Ticket ticket, CancellationToken cancellationToken);
    Task<bool> ExistsWithTitleAsync(string title, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid ticketId, CancellationToken cancellationToken);
}

public sealed class OpenTicketHandler(ITicketRepository repository)
{
    public async Task<TicketDto> HandleAddAsync(
        OpenTicketCommand command, CancellationToken cancellationToken = default)
    {
        string title = NormalizeTicket(command!.Title);
        TicketPriority priority = ParsePriority(command.Priority);

        Ticket ticket = Ticket.Open(title, priority);
        await repository.AddAsync(ticket, cancellationToken);

        return new TicketDto(ticket.Id.Value, ticket.Title, ticket.Priority.ToString());
    }

    public async Task<bool> HandleDeleteAsync(Guid ticketId, CancellationToken cancellationToken = default)
    {
        bool deleted = await repository.DeleteAsync(ticketId, cancellationToken);
        return deleted;
    }

    private static string NormalizeTicket(string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new UseCaseException("Title cannot be null or whitespace.");
        }

        return title.Trim();
    }

    private static TicketPriority ParsePriority(string? value)
    {
        return value?.Trim().ToLowerInvariant() switch
        {
            "low" or "bassa" => TicketPriority.Low,
            "high" or "alta" => TicketPriority.High,
            null or "" or "normal" or "normale" => TicketPriority.Normal,
            _ => throw new UseCaseException($"Priorità non riconosciuta: {value}")
        };
    }
}


public sealed class UseCaseException(string message) : Exception(message);