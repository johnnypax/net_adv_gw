using System;
using System.Collections.Generic;
using System.Text;

namespace OfficeServiceDesk.Core;

public enum TicketPriority
{
    Low, 
    Normal,
    Medium,
    High
}

public readonly record struct TicketId(Guid Value)
{
    public static TicketId New() => new(Guid.NewGuid());
}

public class Ticket
{
    public TicketId Id { get; } // Strong typing

    public string Title { get; }

    public TicketPriority Priority { get; }

    private Ticket(TicketId id, string title, TicketPriority priority)
    {
        Id = id;
        Title = title;
        Priority = priority;
    }

    public static Ticket Open(string title, TicketPriority priority)
    {
        if(string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException("Title cannot be null or whitespace.");
        }

        return new Ticket(TicketId.New(), title, priority);
    }

}

public sealed class DomainException(string message) : Exception(message);
