using System;

namespace Core.Entities;

public class Product : BaseEntity
{
    //require a name so we cannot create a new product without specifying a name
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public required string PictureURL { get; set; }
    public required string Type { get; set; }
    public required string Brand { get; set; }
    public int QuantityInStock { get; set; }
}
