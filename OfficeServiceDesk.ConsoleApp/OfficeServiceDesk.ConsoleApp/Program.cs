using OfficeServiceDesk.Application;
using OfficeServiceDesk.Infrastructure;

ITicketRepository repository = new InMemoryTicketRepository();
var handler = new OpenTicketHandler(repository);

try
{
    var command = new OpenTicketCommand("Fix printer issue", "High");
    TicketDto dto = await handler.HandleAddAsync(command);

    Console.WriteLine($"Ticket created: Id={dto.id}, Title={dto.title}, Priority={dto.priority}");


    if(await handler.HandleDeleteAsync(dto.id))
    {
        Console.WriteLine("OK, ticket deleted successfully.");
    }
    else
    {
        Console.WriteLine("Error");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Unexpected error: {ex.Message}");
}