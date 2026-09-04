
using System.Xml.Linq;

var classA = new ProductClass(" bk-001 ", "Refactoring", 39.90m);
var classB = new ProductClass(" bk-001 ", "Refactoring", 39.90m);

Console.WriteLine($"classA == classB: {classA == classB}");
Console.WriteLine($"classA == classB: {classA.Equals(classB)}");

var recordA = new ProductRecord(" bk-001 ", "Refactoring", 39.90m);
var recordB = new ProductRecord(" bk-001 ", "Refactoring", 39.90m);

Console.WriteLine($"recordA == recordB: {recordA == recordB}");
Console.WriteLine($"recordA == recordB: {recordA.Equals(recordB)}");


var recordC = new ProductRecord(" bk-001 ", "Refactoring", -39.90m);
Console.WriteLine("Fine programma");

public sealed class ProductClass(string sku, string name, decimal price)
{
    public string Sku { get; set; } = sku;
    public string Name { get; set; } = name;
    public decimal Price { get; set; } = price;
}

//public sealed record ProductRecord(string Sku, string Name, decimal Price);


public sealed record ProductRecord(string sku, string name, decimal price)
{
    public string Sku { get; } = Normalize(sku);
    public string Name { get; } = name;
    public decimal Price { 
        get; 
        init => field = value >= 0 ? value : throw new ArgumentException("Il prezzo non può essere negativo", nameof(value));
    } = price >= 0 ? price : throw new ArgumentException("Il prezzo non può essere negativo", nameof(price));

    public static string Normalize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Lo SKU non può essere vuoto", nameof(value));
        }

        return value.Trim();
    }
};
