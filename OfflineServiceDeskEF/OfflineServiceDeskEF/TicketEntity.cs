using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OfflineServiceDeskEF;

public sealed class TicketEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Priority { get; set; } = "normal";
    public string Status { get; set; } = "open";
    public DateTimeOffset CreatedAt { get; set; }
    public List<NoteEntity> Notes { get; set; } = [];
}

public sealed class NoteEntity
{
    public Guid Id { get; set; }
    public Guid TicketId {  get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public TicketEntity Ticket { get; set; } = null!;

}
