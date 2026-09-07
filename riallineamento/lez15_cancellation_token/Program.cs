
using var cancellationTokenSource = new CancellationTokenSource();
CancellationToken token = cancellationTokenSource.Token;

Task work = ExecuteWork(token);

await Task.Delay(5000);

Console.WriteLine("Richiesta cancellazione");
cancellationTokenSource.Cancel();

await Task.Delay(5000);

Console.WriteLine("Programma principale terminato (main)");


static async Task ExecuteWork(CancellationToken cancellationToken)
{
    for(int i=0; i<10; i++)
    {
        Console.WriteLine($"Esecuzione dell'elemento {i}");
        await Task.Delay( 1000 , cancellationToken);
    }

    Console.WriteLine("Lavoro completato");
}