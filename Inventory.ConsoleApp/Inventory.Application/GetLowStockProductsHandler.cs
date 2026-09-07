using Inventory.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application;
public sealed class GetLowStockProductsHandler(IProductRepository productRepository)
{
    public async Task<IReadOnlyCollection<ProductDto>> HandleAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IReadOnlyCollection<Product> products = 
            await productRepository.GetLowStockProductsAsync(cancellationToken);

        return products.Select(p => new ProductDto(
            p.Id.Value,
            p.Name,
            p.Quantity,
            p.MinimumStock,
            p.IsLowSock)).ToList();
    }
}