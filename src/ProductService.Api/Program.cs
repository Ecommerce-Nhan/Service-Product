using ProductService.Api.Extensions;
using Serilog;
using Serilog.Debugging;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

try
{
    var builder = WebApplication.CreateBuilder(args);
    Log.Logger = new LoggerConfiguration()
       .ReadFrom.Configuration(builder.Configuration)
       //.WriteTo.Elasticsearch(
       //     new ElasticsearchSinkOptions(
       //        new Uri("https://product-c309be.es.ap-southeast-1.aws.elastic.cloud"))
       //        {
       //            ApiKey = "MUJaMzM1TUJ0TUVhM0tlTWVzRkE6LUdjZWU1RlpRLU9MekN4VkxVeTh0UQ==",
       //            AutoRegisterTemplate = true,
       //            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
       //            IndexFormat = "product-service-{0:yyyy.MM.dd}",
       //            CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
       //            EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
       //                               EmitEventFailureHandling.WriteToFailureSink |
       //                               EmitEventFailureHandling.RaiseCallback,
       //        })
       .CreateLogger();

    SelfLog.Enable(msg => Log.Information(msg));
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
