using Inventory.Application;
using Inventory.Core;
using Inventory.Infrastructure;

IProductRepository repository = new InMemoryRepository();

// Composition root
RegisterProductHandler registerProductHandler = new(repository);
RemoveStockHandler removeStockHandler = new(repository);
GetLowStockProductsHandler getLowStockProductsHandler = new(repository);

ProductDto keyboard =
    await registerProductHandler.HandleAsync(
        "Tastiera Wireless",
        10,
        5);

ProductDto mouse =
    await registerProductHandler.HandleAsync(
        "Mouse Wireless",
        3,
        5);

Console.WriteLine("Ok, prodotti inseriti");

Console.WriteLine(
    $"{keyboard.Id} | " +
    $"{keyboard.Name} | " +
    $"Quantità: {keyboard.Quantity}");

Console.WriteLine(
    $"{mouse.Id} | " +
    $"{mouse.Name} | " +
    $"Quantità: {mouse.Quantity}");

Console.WriteLine("---------------------------");
Console.WriteLine("PRELIEVO MAGAZZINO");

ProductId productId = new ProductId(keyboard.Id);
ProductDto updatedKeyboard = await removeStockHandler.HandleAsync(productId, 6);

Console.WriteLine(
    $"{updatedKeyboard.Id} | " +
    $"{updatedKeyboard.Name} | " +
    $"Quantità: {updatedKeyboard.Quantity}");


Console.WriteLine("---------------------------");
Console.WriteLine("LOW STOCK");

IReadOnlyCollection<ProductDto> lowStockProducts =
    await getLowStockProductsHandler.HandleAsync();

foreach (ProductDto product in lowStockProducts)
{
    Console.WriteLine($"{product.Id} {product.Name} {product.Quantity}");
}