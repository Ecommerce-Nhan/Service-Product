using Orchestration.ServiceDefaults;
using ProductService.Api.Extensions;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);
    GeneralServiceExtensions.ConfigureSerilog(builder);

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