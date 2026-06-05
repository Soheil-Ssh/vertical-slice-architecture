using Scalar.AspNetCore;
using VerticalSliceArchitecture.Api.Common.Behaviors;
using VerticalSliceArchitecture.Api.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add application db context
services.AddDbContext<ApplicationDbContext>(option => 
    option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Add MediatR
services.AddMediatR(options =>
    options.RegisterServicesFromAssemblyContaining(typeof(Program)));

// Add Fluent validation
services.AddValidatorsFromAssembly(typeof(Program).Assembly);

// Add Validation behavior
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddCarter();
services.AddExceptionHandler<ValidationExceptionHandler>();
services.AddExceptionHandler<GlobalExceptionHandler>();
services.AddProblemDetails();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.MapCarter();
app.Run();
