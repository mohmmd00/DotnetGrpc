using MRS.Application;
using MRS.Infrastructure.GRPC.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSingleton<MessageRouterApplication>(); // Register MessageRouterApplication in DI

// Configure logging.
builder.Logging.ClearProviders(); // Clear default providers if needed
builder.Logging.AddConsole();     // Add console logging
builder.Logging.SetMinimumLevel(LogLevel.Information); // Set minimum log level

// Optional: Add Serilog for advanced logging
// var loggerConfiguration = new LoggerConfiguration()
//     .WriteTo.Console()
//     .MinimumLevel.Information();
// Log.Logger = loggerConfiguration.CreateLogger();
// builder.Logging.AddSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();

// Map gRPC service
app.MapGrpcService<RouterService>();

// Map a simple GET endpoint for informational purposes
app.MapGet("/", () =>
{
    return Results.Content(
        "Communication with gRPC endpoints must be made through a gRPC client. " +
        "To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909",
        "text/plain"
    );
});

// Run the application
app.Run();