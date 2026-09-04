#region Parsing con Lambda e delegate
TryParse<int> parseInt = (text, out value) => int.TryParse(text, out value);

string[] inputs = ["10", "errore", "30"];

foreach (string input in inputs)
{
    string message = Format(input, parseInt);
    Console.WriteLine(message);
}

static string Format<T>(string text, TryParse<T> tryParse)
{
    Func<T, string> success = static value => $"Parsed value: {value}";

    return tryParse(text, out T? parsed) ? success(parsed) : $"Failed to parse '{text}'";
}

public delegate bool TryParse<T>(string text, out T value);
#endregion

int threshold = 10;
Func<int, bool> isGreaterThanThreshold = value => value > threshold;

Console.WriteLine(isGreaterThanThreshold(50));
