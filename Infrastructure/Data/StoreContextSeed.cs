using System;
using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        //first make sure we have no data before seeding
        if (!context.Products.Any())
        {
            var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");

            //deserialize from jason into product class
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);

            //if products is null return out of here
            if (products == null) return;

            //tell entity framework to track all the products that are going to be added
            context.Products.AddRange(products);

            //save the products changes  
            await context.SaveChangesAsync();
        }
    }
}
