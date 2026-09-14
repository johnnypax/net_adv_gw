using Microsoft.EntityFrameworkCore;
using OfflineServiceDeskEF;

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
    //bool created = await dbContext.Database.EnsureCreatedAsync();

    //if (!created)
    //{
    //    Console.WriteLine("Not created");
    //    return;
    //}
    //Controllo che non ci siano migrazioni da fare!
    try
    {
        await dbContext.Database.MigrateAsync();
        Console.WriteLine("Migrations applied");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Nothing to migrate: " + ex.Message);
    }


var ticket = new TicketEntity
{
    Id = Guid.NewGuid(),
    Title = "Verifica il backup della filiale",
    Priority = "high",
    Status = "open",
    CreatedAt = DateTimeOffset.UtcNow,
    Operator = "Giovanni Pace"
};

    var repository = new EFTicketRepository(dbContext);

    await repository.AddAsync(ticket);
    Console.WriteLine("Stappoooooooo");
