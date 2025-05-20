using System;
using Core.Entities;
using Infrastructure.Config;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

//derive from entity framework core DbContext class
//use primary constructor which is new in c# ( StoreContext(DbContextOptions options) : DbContext(options) ) in class header
public class StoreContext(DbContextOptions options) : DbContext(options)
{
    //this tells entity framework to call the table products when we create a new migration
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //tell our store context where the configuration we set up (i.e. 2 decimal places for price) are located in this case in our ProductConfigurations.cs class as long as it is in this same assembly (infrastucture assembly)
        modelBuilder.ApplyConfigurationsFromAssembly( typeof(ProductConfiguration).Assembly );
    }

}
