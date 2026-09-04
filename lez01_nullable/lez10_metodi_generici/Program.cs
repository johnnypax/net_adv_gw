using System.Numerics;

try
{
    int[] voti = [10, 20, 30];

    decimal[] votiDecimali = [10.5m, 20.5m, 30.5m];

    Console.WriteLine(Average(voti));
    Console.WriteLine(Average(votiDecimali));
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}

static T Average<T>(ReadOnlySpan<T> values) where T : INumber<T> {

    if(values.IsEmpty)
        throw new ArgumentException("The collection is empty.", nameof(values));

    T sum = T.Zero;

    foreach (T item in values)
        sum += item;

    return sum / T.CreateChecked(values.Length);
}


//public interface IParsableEntity<TSelf> where TSelf : IParsableEntity<TSelf>
//{
//    static abstract TSelf Parse(string text);
//}