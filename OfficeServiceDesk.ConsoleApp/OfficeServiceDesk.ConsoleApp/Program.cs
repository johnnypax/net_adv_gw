using OfficeServiceDesk.Application;
using OfficeServiceDesk.Infrastructure;

ITicketRepository repository = new InMemoryTicketRepository();
var handler = new OpenTicketHandler(repository);

try
{
    var command = new OpenTicketCommand("Fix printer issue", "High");
    TicketDto dto = await handler.HandleAsync(command);

    Console.WriteLine($"Ticket created: Id={dto.id}, Title={dto.title}, Priority={dto.priority}");
}
catch (Exception ex)
{
    Console.WriteLine($"Unexpected error: {ex.Message}");
}