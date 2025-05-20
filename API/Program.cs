using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//SERVICES

builder.Services.AddControllers();
//add our Store Context service and pass our connection string from our config file called DefaultConnection
builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//scoped to the lifetime of the current http request and disposes after the request is done
//since we have a repository and an implementation class we need to specify both
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

//MIDDLEWARE - software that can run on the request or response

// Configure the HTTP request pipeline.

app.MapControllers();

//do any migrations and seed database
try
{
    //the using statement means that any code we use with this variable will be disposed of after this runs since we are creating a scope
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreContext>();

    //applies any pending migrations or creates db if it doesn't already exist
    await context.Database.MigrateAsync();
    //seed the data
    await StoreContextSeed.SeedAsync(context);

}
catch (System.Exception ex)
{
    Console.WriteLine(ex);
    throw;
}

app.Run();
