using Inventory.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application;
public sealed class RemoveStockHandler(IProductRepository productRepository)
{
    public async Task<ProductDto> HandleAsync(
        ProductId productId,
        int quantityToRemove,
        CancellationToken cancellationToken = default)
    {

        cancellationToken.ThrowIfCancellationRequested();
        Product? product = await productRepository.FindByIdAsync(productId, cancellationToken);
    
        if(product is null)
            throw new UseCaseException($"Product with ID {productId.Value} not found.");

        product.RemoveStock(quantityToRemove);

        await productRepository.SaveAsync(product, cancellationToken);

        return new ProductDto(
            product.Id.Value, 
            product.Name, 
            product.Quantity, 
            product.MinimumStock, 
            product.IsLowSock);
    }
}