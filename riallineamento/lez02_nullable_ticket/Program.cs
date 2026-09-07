using lez02_nullable_ticket;

Ticket[] tickets =
[
    new("TK-101", "Stampante offline", "Marta"),
    new("TK-102", "Password da reimpostare", null)
];

var repository = new TicketRepository(tickets);

ShowTicket(repository, "TK-102");
ShowTicket(repository, "TK-999");

static void ShowTicket(TicketRepository repository, string code)
{
    // out: il metodo scrive qui un risultato aggiuntivo oltre al valore restituito.
    if (!repository.TryFind(code, out Ticket? ticket))
    {
        Console.WriteLine($"{code}: ticket non trovato");
        return;
    }
    // L'attributo sul metodo rende ticket not-null in questo ramo.
    string assigneeLabel = ticket.Assignee ?? "non assegnato";

    Console.WriteLine(
        $"{ticket.Code}: {ticket.Title} - {assigneeLabel}");
}
