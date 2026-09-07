#region Primo esempio
//string? rawName = Console.ReadLine();

////guard
//if(string.IsNullOrWhiteSpace(rawName))
//{
//    Console.WriteLine("Invalid name provided.");
//    return;
//}

//string displayName = rawName.Trim();
//Console.WriteLine($"{displayName}");
#endregion

string displayName = Console.ReadLine()?.Trim() ?? "ospite";
Console.WriteLine($"Ciao {displayName}");

