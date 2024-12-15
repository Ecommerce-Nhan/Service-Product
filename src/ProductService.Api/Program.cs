using ProductService.Api.Extentions;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);
 
    Log.Logger = new LoggerConfiguration()
       .ReadFrom.Configuration(builder.Configuration)
       .CreateLogger();

    Log.Information("Starting server.");

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
