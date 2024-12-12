using ProductService.Api.Extentions;
using Serilog;

try
{
    Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
    Log.Information("Starting server.");

    var builder = WebApplication.CreateBuilder(args);

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline(builder);

    app.Run();
} 
catch (Exception ex)
{
    Log.Fatal(ex, "server terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
