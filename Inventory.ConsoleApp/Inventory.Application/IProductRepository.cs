using Inventory.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application;
public interface IProductRepository
{
    Task AddAsync(Product product, CancellationToken cancellationToken);
    Task<bool> ExistsByName(string name, CancellationToken cancellationToken);
    Task<Product?> FindByIdAsync(ProductId productId, CancellationToken cancellationToken);
    Task SaveAsync(Product product, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Product>> GetLowStockProductsAsync(CancellationToken cancellationToken);
}
