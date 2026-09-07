using Inventory.Application;
using Inventory.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Infrastructure;
public sealed class InMemoryRepository : IProductRepository
{
    private readonly List<Product> _products = [];

    public Task<bool> ExistsByName(
        string name,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        bool exists = _products.Any(product =>
            string.Equals(
                product.Name,
                name,
                StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(exists);
    }

    public Task AddAsync(
       Product product,
       CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _products.Add(product);

        return Task.CompletedTask;
    }

    public Task<Product?> FindByIdAsync(
        ProductId id,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        Product? product = _products
            .FirstOrDefault(product => product.Id == id);

        return Task.FromResult(product);
    }

    public Task SaveAsync(
        Product product,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _products.Remove(product);
        _products.Add(product);

        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<Product>> GetLowStockProductsAsync(
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IReadOnlyCollection<Product> products =
            _products
                .Where(product => product.IsLowSock)
                .ToList();

        return Task.FromResult(products);
    }
}