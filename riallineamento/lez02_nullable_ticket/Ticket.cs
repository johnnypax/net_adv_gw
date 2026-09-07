using System;
using System.Collections.Generic;
using System.Text;

namespace lez02_nullable_ticket
{
    public sealed class Ticket
    {
        public string Code { get; } = null!;

        public string? Assignee { get; }

        public string? Title { 
            get; 
            set => field = Ticket.RequireText(value, nameof(Title)); }

        private static string RequireText(string? value, string parameterName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, parameterName);
            return value.Trim();
        }

        public Ticket(string code, string title, string? assignee)
        {
            Code = RequireText(code, nameof(code));
            Title = title;
            Assignee = string.IsNullOrWhiteSpace(assignee)
                ? null
                : assignee.Trim();
        }
    }
}
