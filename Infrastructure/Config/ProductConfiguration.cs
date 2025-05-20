using System;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Config;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> builder)
    {
        //tell entity frame work that the price column inside our product is 2 decimal places
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
    }
}
