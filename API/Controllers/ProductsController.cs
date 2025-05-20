using System;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;


//this lets it to know look in the body of the requests(automatic model binding) as well as other things
//
[ApiController]
//this says the route should be the name of the controller minus the word controller so in this case Products
[Route("api/[controller]")]
//derive from controller base class through MVC without view support
//use dependency injection to inject the store context into the class
public class ProductsController(StoreContext context) : ControllerBase
{

    [HttpGet]
    //Action result let us get responses and we are telling it that is will be a list of products
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        //await the response and then asynchronously make it a list
        return await context.Products.ToListAsync();
    }

    //this get request is for one product with an id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        //get the product that matches the id that is passed in
        var product = await context.Products.FindAsync(id);

        if (product == null) return NotFound();

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        //add this new product to our db
        context.Products.Add(product);

        //save the changes
        await context.SaveChangesAsync();

        return product;
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        //if the id passed in does not match the id of the product that was passed in or if the product does not exist then return a bad request
        if (product.Id != id || !ProductExists(id)) return BadRequest("cannot update this product");

        //tell entity framework what we are updating and for it to track it
        context.Entry(product).State = EntityState.Modified;

        await context.SaveChangesAsync();

        //since this is an update we just return an ok it worked since there is no content to return
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await context.Products.FindAsync(id);

        if (product == null) return NotFound();

        //tell entity framework to track this removal
        context.Products.Remove(product);

        //save the removal
        await context.SaveChangesAsync();

        //since this is an delete we just return an ok it worked since there is no content to return
        return NoContent();
    }

    private bool ProductExists(int id)
    {
        //see if any products exist in our db with an id that matches the passed in id
        return context.Products.Any(x => x.Id == id);
    }
}
