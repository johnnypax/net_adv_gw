using System;
using System.Collections.Generic;
using System.Text;

namespace operazioni_file.Models
{
    public sealed record Ticket(string ExternalId, string Title, string Priority)
}
