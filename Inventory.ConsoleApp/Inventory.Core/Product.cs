using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Core;

public readonly record struct ProductId(Guid Value)
{
    public static ProductId New() => new(Guid.NewGuid());
}

public sealed class Product
{
    public ProductId Id { get; }
    public string Name { get; }
    public int Quantity { get; private set; }
    public int MinimumStock { get; }

    public bool IsLowSock => Quantity < MinimumStock;

    private Product(ProductId id, string name, int quantity, int minimumStock)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        MinimumStock = minimumStock;
    }

    public static Product Create(string name, int quantity, int minimumStock)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name cannot be null or whitespace.");

        if(name.Length < 3)
            throw new DomainException("Product name must be at least 3 characters long.");

        if (quantity < 0)
            throw new DomainException("Product quantity cannot be negative.");

        if(minimumStock < 0)
            throw new DomainException("Product minimum stock cannot be negative.");

        return new Product(ProductId.New(), name, quantity, minimumStock);
    }

    public void RemoveStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity to remove must be greater than zero.");
        if (quantity > Quantity)
            throw new DomainException("Cannot remove more stock than available.");

        Quantity -= quantity;
    }
}

public sealed class DomainException(string message) : Exception(message);