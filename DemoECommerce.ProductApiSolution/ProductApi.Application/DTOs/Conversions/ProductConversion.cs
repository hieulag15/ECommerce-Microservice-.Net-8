using ProductApi.Domain.Entity;

namespace ProductApi.Application.DTOs.Conversions;

public static class ProductConversion
{
    public static Product ToEntity(ProductDTO product) => new Product()
    {
        Id = product.Id,
        Name = product.Name,
        Price = product.Price,
        Quantity = product.Quantity
    };

    public static (ProductDTO?, IEnumerable<ProductDTO>?) FromEntity(Product product, IEnumerable<Product>? products)
    {
        // single return
        if (product is not null || products is null)
        {
            var singleProduct = new ProductDTO
                (
                product!.Id,
                product.Name!,
                product.Price,
                product.Quantity
                );
            return (singleProduct, null);
        }

        // list return
        if (product is null || products is not null)
        {
            var listProducts = products.Select(p =>
                new ProductDTO(p.Id, p.Name!, p.Price, p.Quantity)).ToList();
            return (null, listProducts);
        }
        return (null, null);
    }
}
