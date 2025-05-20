using System;
using Core.Entities;

namespace Core.Interfaces;

public interface IProductRepository
{
    //can optionally select a product by type or brand
    Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort);
    //this could return a product or be null
    Task<Product?> GetProductByIdAsync(int id);
    //get list of brands
    Task<IReadOnlyList<String>> GetBrandsAsync();
    //get list of types
    Task<IReadOnlyList<String>> GetTypesAsync();
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);
    bool ProductExists(int id);
    //this is to check if something was saved in our db
    Task<bool> SaveChangesAsync();
}
