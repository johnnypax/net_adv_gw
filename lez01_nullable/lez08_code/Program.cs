#region Semplice combo Queue HashSet
//Queue<string> waiting = new();
//HashSet<string> completed = new();

//Add("Task 1");
//Add("Task 1");
//Add("Task 3");

//while(waiting.TryDequeue(out string? ticket))
//{
//    Console.WriteLine($"Processing {ticket}...");
//}

//void Add(string ticket)
//{
//    if (!completed.Add(ticket))
//    {
//        Console.WriteLine($"Ticket {ticket} already inserted.");
//        return;
//    }

//    waiting.Enqueue(ticket);
//    Console.WriteLine($"Ticket {ticket} added to the waiting queue.");
//}
#endregion

Catalog<Product> catalog = new();

try
{
    catalog.Add(new Product("P001", "Mouse", 12.99m));
    catalog.Add(new Product("P001", "Laptop", 999.99m));
}
catch (InvalidOperationException ex)
{
    Console.WriteLine(ex.Message);
}

if(catalog.TryGet("P001", out Product? product))
{
    Console.WriteLine($"Product found: {product!.Name} - ${product.Price}");
}
else
{
    Console.WriteLine("Product not found.");
}

Console.WriteLine(catalog.Count);

public sealed record Product(string Id, string Name, decimal Price) : IHasId;

public interface IHasId
{
    string Id { get; }
}

public sealed class Catalog<T> where T : IHasId
{
    private readonly Dictionary<string, T> _items = new(StringComparer.OrdinalIgnoreCase);
    
    public int Count => _items.Count;

    public void Add(T item)
    {
        if (!_items.TryAdd(item.Id, item))
        {
            throw new InvalidOperationException($"Item with ID '{item.Id}' already exists.");
        }
    }

    public bool TryGet(string id, out T? item)
    {
        return _items.TryGetValue(id, out item);
    }

    public IEnumerable<T> GetAll()
    {
        return _items.Values;
    }
}   
