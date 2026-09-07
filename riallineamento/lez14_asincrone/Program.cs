// Task<T> - await

using System.Diagnostics;

Stopwatch timer = Stopwatch.StartNew();

// Start A -> await A -> start B -> await B
string first = await LoadAsync("first", 1000);
string second = await LoadAsync("second", 1000);

Console.WriteLine($"Valore del timer in serie: {timer.ElapsedMilliseconds} ms");

timer.Restart();

// Start A -> Start B -> await entrambi
Task<string> third = LoadAsync("third", 1000);
Task<string> fourth = LoadAsync("fourth", 1000);

string[] result = await Task.WhenAll(third, fourth);    //Attende che entrambi i task siano completati

Console.WriteLine($"Valore del timer in concorrenza: {timer.ElapsedMilliseconds} ms");
Console.Write(string.Join(", ", result));



static async Task<string> LoadAsync(string id, int delayMilliseconds)
{
    await Task.Delay(delayMilliseconds);
    return $"Data for {id}";
}