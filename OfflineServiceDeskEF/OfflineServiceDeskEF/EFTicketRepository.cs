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
}
