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

var app = builder.Build();

//MIDDLEWARE - software that can run on the request or response

// Configure the HTTP request pipeline.

app.MapControllers();

app.Run();
