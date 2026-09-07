using Inventory.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application;
public sealed class RegisterProductHandler(IProductRepository productRepository)
{
    public async Task<ProductDto> HandleAsync(
        string name, 
        int initialQuantity, 
        int minimumStock, 
        CancellationToken cancellationToken = default)
    {

        cancellationToken.ThrowIfCancellationRequested();

        Product product = Product.Create(name, initialQuantity, minimumStock);

        bool exists = await productRepository.ExistsByName(name, cancellationToken);

        if (exists)
            throw new UseCaseException("Product with the same name already exists.");

        await productRepository.AddAsync(product, cancellationToken);

        return new ProductDto(
            product.Id.Value,
            product.Name,
            product.Quantity,
            product.MinimumStock,
            product.IsLowSock
        );
    }
}

