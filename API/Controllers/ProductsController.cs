using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;


//this lets it to know look in the body of the requests(automatic model binding) as well as other things
//this also automatically looks in the query string parameters if we pass in strings to a function like with our GetProducts, if we didn't put [ApiController] up here then we would need to add [FromQuery] in front of all our string parameters
[ApiController]
//this says the route should be the name of the controller minus the word controller so in this case Products
[Route("api/[controller]")]
//derive from controller base class through MVC without view support
//use dependency injection to inject the repository into the class
public class ProductsController(IProductRepository repo) : ControllerBase
{

    [HttpGet]
    //Action result let us get responses and we are telling it that is will be a list of products
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        //await using our repo methods
        return Ok(await repo.GetProductsAsync(brand, type, sort));
    }

    //this get request is for one product with an id
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        //get the product that matches the id that is passed in
        var product = await repo.GetProductByIdAsync(id);

        if (product == null) return NotFound();

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        //add this new product to our db
        repo.AddProduct(product);

        //return an action that gives a ok response if the product is good using our getProduct action (method above) this also gives a location header of where the resource can be obtained
        if (await repo.SaveChangesAsync()) return CreatedAtAction("GetProduct", new { id = product.Id }, product);

        return BadRequest("Problem creating product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        //if the id passed in does not match the id of the product that was passed in or if the product does not exist then return a bad request
        if (product.Id != id || !ProductExists(id)) return BadRequest("cannot update this product");

        repo.UpdateProduct(product);

        //since this is an update we just return an ok it worked since there is no content to return
        if (await repo.SaveChangesAsync()) return NoContent();


        return BadRequest("Problem updating the product.");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetProductByIdAsync(id);

        if (product == null) return NotFound();

        repo.DeleteProduct(product);

        //since this is an delete we just return an ok it worked since there is no content to return
        if (await repo.SaveChangesAsync()) return NoContent();

        return BadRequest("Problem deleting the product.");
    }

    //API/Products/brands
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        return Ok(await repo.GetBrandsAsync());
    }

    //API/Products/types
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        return Ok(await repo.GetTypesAsync());
    }

    private bool ProductExists(int id)
    {
        //see if any products exist in our db with an id that matches the passed in id
        return repo.ProductExists(id);
    }
}
