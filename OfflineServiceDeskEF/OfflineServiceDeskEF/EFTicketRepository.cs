using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace OfflineServiceDeskEF;

public sealed class EFTicketRepository(ServiceDeskDbContext dbContext)
{
    public async Task AddAsync(TicketEntity ticket, CancellationToken cancellationToken = default)
    {
        dbContext.Tickets.Add(ticket);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    //Input testo del Ticket e se Open
    public async Task<IReadOnlyList<TicketListItem>> SearchOpenAsync(
        string? searchTerm, CancellationToken cancellationToken = default)
    {
        IQueryable<TicketEntity> query = dbContext.Tickets
            .AsNoTracking()
            .Where(ticket => ticket.Status == "open");

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string normalizedSearchTerm = searchTerm.Trim();
            query = query.Where(ticket => ticket.Title.Contains(normalizedSearchTerm));
        }

        return await query
            .OrderByDescending(ticket => ticket.Priority)
            .ThenBy(ticket => ticket.Title)
            .Select(ticket =>
                new TicketListItem(
                    ticket.Id,
                    ticket.Title,
                    ticket.Priority,
                    ticket.Status))
            .ToListAsync(cancellationToken);
    }

    public async Task CloseWithNotesAsync(Guid Id, string noteText, CancellationToken cancellationToken = default)
    {
        TicketEntity? ticket = 
            await dbContext.Tickets.SingleOrDefaultAsync(t => t.Id == Id, cancellationToken);

        if (ticket is null)
        {
            throw new InvalidOperationException("Ticket not found");
        }

        await using var transaction = 
            await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            ticket.Status = "closed";
            //await dbContext.SaveChangesAsync(cancellationToken);

            NoteEntity note = new NoteEntity
            {
                Id = Guid.NewGuid(),
                TicketId = ticket.Id,
                Text = noteText,
                CreatedAt = DateTimeOffset.UtcNow
            };

            dbContext.Notes.Add(note);

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

        } catch
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
        

    }
}
