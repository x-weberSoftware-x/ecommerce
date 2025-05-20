using System;
using System.Security;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

//use primary constructor dependency injection for our store context
public class ProductRepository(StoreContext context) : IProductRepository
{
    public void AddProduct(Product product)
    {
        context.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        context.Products.Remove(product);
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync()
    {   
        //get a distinct list of brands
        return await context.Products.Select(x => x.Brand).Distinct().ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
    {
        //build a query for entity framework
        var query = context.Products.AsQueryable();

        //check if we have a brand passed in, if we do then add to the query a where clause based on the passed in brand
        if (!string.IsNullOrWhiteSpace(brand)) query = query.Where(x => x.Brand == brand);

        //check if we have a type passed in, if we do then add to the query a where clause based on the passed in type
        if (!string.IsNullOrWhiteSpace(type)) query = query.Where(x => x.Type == type);

        //check our sorting parameter
        query = sort switch
        {
            //if we passed in priceAsc then sort by price ascending
            "priceAsc" => query.OrderBy(x => x.Price),
            //if we passed in priceDesc then sort by price descending
            "priceDesc" => query.OrderByDescending(x => x.Price),
            //else our default is to sort by name ascending
            _ => query.OrderBy(x => x.Name)
        };
        
        //return the list from the query
        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetTypesAsync()
    {
        //get a distinct list of types
        return await context.Products.Select(x => x.Type).Distinct().ToListAsync();
    }

    public bool ProductExists(int id)
    {
        return context.Products.Any(p => p.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        //save changes async returns a 1 or 0 if changes where saved so we want to return true if that function returned a 1 
        return await context.SaveChangesAsync() > 0;
    }

    public void UpdateProduct(Product product)
    {
        //tell entity framework to track the change
        context.Entry(product).State = EntityState.Modified;
    }
}
