using FluentValidation;
using ProductService.Application.Mappers;
using ProductService.Infrastructure;
using Serilog;
using Serilog.Debugging;

namespace ProductService.Api.Extensions;

internal static partial class HostingExtensions
{
    public static void ConfigureSerilog(WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration)
                //    .WriteTo.Elasticsearch(
                //         new ElasticsearchSinkOptions(
                //            new Uri("https://product-c309be.es.ap-southeast-1.aws.elastic.cloud"))
                //         {
                //             ApiKey = "MUJaMzM1TUJ0TUVhM0tlTWVzRkE6LUdjZWU1RlpRLU9MekN4VkxVeTh0UQ==",
                //             AutoRegisterTemplate = true,
                //             AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                //             IndexFormat = "product-service-{0:yyyy.MM.dd}",
                //             CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
                //             EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                //                                   EmitEventFailureHandling.WriteToFailureSink |
                //                                   EmitEventFailureHandling.RaiseCallback,
                //         })
                .CreateLogger();

        SelfLog.Enable(msg => Log.Information(msg));
        Log.Information("Starting server.");
    }
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // Host Configuration
        builder.Host.UseSerilog();
        builder.Host.AddAutoFacConfiguration();

        // Default Configuration
        builder.Services.AddRedis();
        builder.Services.AddHttpClient();
        builder.Services.AddControllers();
        builder.Services.AddAutoMapper(typeof(ProductAutoMapperProfile).Assembly);
        builder.Services.AddValidatorsFromAssembly(typeof(ProductAutoMapperProfile).Assembly);

        // Custom Configuration
        builder.Services.AddHandleException();
        builder.Services.AddSwaggerConfiguration();
        builder.Services.AddMediatRConfiguration();
        builder.Services.AddJWTConfiguration(builder.Configuration);
        builder.Services.AddAWSConfiguration(builder.Configuration);
        builder.Services.AddDatabaseConfiguration(builder.Configuration);
        builder.Services.AddHangfireConfiguration(builder.Configuration);
        builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();

        return builder.Build();
    }
}