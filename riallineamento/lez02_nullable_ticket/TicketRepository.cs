using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace lez02_nullable_ticket
{
    public sealed class TicketRepository
    {
        private readonly Dictionary<string, Ticket> _ticket;

        public TicketRepository(IEnumerable<Ticket> tick) => 
            _ticket = new Dictionary<string, Ticket>(tick.ToDictionary(t => t.Code));

        public bool TryFind(
            string code, 
            [NotNullWhen(true)] out Ticket? ticket) 
            => _ticket.TryGetValue(code, out ticket);

        public Ticket? Find(string code) =>
            _ticket.GetValueOrDefault(code);
    }
}
