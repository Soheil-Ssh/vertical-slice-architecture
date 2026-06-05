using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add application db context
services.AddDbContext<ApplicationDbContext>(option => 
    option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Add MediatR
services.AddMediatR(options =>
    options.RegisterServicesFromAssemblyContaining(typeof(Program)));

// add Auto mapper profiles
services.AddAutoMapper(cfg => { }, typeof(Program).Assembly);

// Add Fluent validation
services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddCarter();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapCarter();

app.Run();
