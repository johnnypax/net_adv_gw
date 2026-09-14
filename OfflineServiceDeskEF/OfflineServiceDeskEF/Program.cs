using Microsoft.EntityFrameworkCore;
using OfflineServiceDeskEF;
using System.Net.Sockets;

//string demoDirectory = Path.Combine(Path.GetTempPath(), "OfflineServiceDeskEF");
//Directory.CreateDirectory(demoDirectory);

//string databasePath = Path.Combine(demoDirectory, "serviceDesk.db");
//#if DEBUG
//if (File.Exists(databasePath))
//{
//    File.Delete(databasePath);  //Attenzione, creare profilo di INIT
//}
//#endif

var options = new DbContextOptionsBuilder<ServiceDeskDbContext>()
            .UseSqlite($"Data source=serviceDesk.db")
            .EnableDetailedErrors()
            .Options;

    await using var dbContext = new ServiceDeskDbContext(options);

    try
    {
        await dbContext.Database.MigrateAsync();
        Console.WriteLine("Migrations applied");

        var repository = new EFTicketRepository(dbContext);

    #region Inserimento dei ticket
    //var ticket = new TicketEntity
    //{
    //    Id = Guid.NewGuid(),
    //    Title = "Sistemare Scrivania",
    //    Priority = "low",
    //    Status = "closed",
    //    CreatedAt = DateTimeOffset.UtcNow
    //};

    //await repository.AddAsync(ticket);
    //Console.WriteLine("Stappoooooooo");
    #endregion

    #region Conteggio Ticket
    //IReadOnlyList<TicketListItem> list = await repository.SearchOpenAsync(null);
    //Console.WriteLine(list.Count.ToString());
    #endregion


    var ticket = new TicketEntity
    {
        Id = Guid.NewGuid(),
        Title = "Sistemare il programma",
        Priority = "high",
        Status = "open",
        CreatedAt = DateTimeOffset.UtcNow
    };

    await repository.AddAsync(ticket);
    Console.WriteLine($"Creato ticket {ticket.Id} {ticket.Status}");

    await Task.Delay(5000);

    await repository.CloseWithNotesAsync(ticket.Id, "Chiuso con successo, modifica struttura");

    Console.WriteLine($"Chiuso ticket {ticket.Id} {ticket.Status}");


}
catch (Exception ex)
    {
        Console.WriteLine("Nothing to migrate: " + ex.Message);
    }
