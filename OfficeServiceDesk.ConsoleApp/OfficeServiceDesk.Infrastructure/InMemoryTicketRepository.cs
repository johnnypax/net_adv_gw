using OfficeServiceDesk.Application;
using OfficeServiceDesk.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OfficeServiceDesk.Infrastructure
{
    public sealed class InMemoryTicketRepository : ITicketRepository
    {
        private readonly List<Ticket> _tickets = [];

        public Task AddAsync(Ticket ticket, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _tickets.Add(ticket);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsWithTitleAsync(string title, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            bool exists = _tickets
                .Any(ticket => string.Equals(ticket.Title, title, StringComparison.OrdinalIgnoreCase));

            return Task.FromResult(exists);
        }
    }
}
