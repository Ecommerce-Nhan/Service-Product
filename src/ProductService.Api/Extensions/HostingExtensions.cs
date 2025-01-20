using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Mappers;
using SharedLibrary.CQRS;
using ProductService.Infrastructure;
using Serilog;
using SharedLibrary.Extentions;
using Hangfire;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using SharedLibrary.Dtos.HealthChecks;
using Newtonsoft.Json;
using Serilog.Debugging;

namespace ProductService.Api.Extensions;

internal static class HostingExtensions
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
        builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();
        builder.Services.AddControllers();
        builder.Services.AddHttpClient();
        builder.Services.AddValidatorsFromAssembly(typeof(BaseRequest).Assembly);
        builder.Services.AddAutoMapper(typeof(ProductAutoMapperProfile).Assembly);

        // Custom Configuration
        builder.Services.AddHandleException();
        builder.Services.AddSwaggerConfiguration();
        builder.Services.AddMediatRConfiguration();
        builder.Services.AddRedisCacheConfiguration();
        builder.Services.AddAWSConfiguration(builder.Configuration);
        builder.Services.AddDatabaseConfiguration(builder.Configuration);
        builder.Services.AddHangfireConfiguration(builder.Configuration);

        return builder.Build();
    }
    public static WebApplication ConfigurePipeline(this WebApplication app, WebApplicationBuilder builder)
    {
        app.ConfigureDevelopment();
        app.CheckHealthy();
        app.UseExceptionHandler("/error");
        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();
        app.MapControllers();

        return app;
    }
    private static WebApplication CheckHealthy(this WebApplication app)
    {
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                var response = new HealthCheckResponse
                {
                    Status = report.Status.ToString(),
                    HealthChecks = report.Entries.Select(x => new IndividualHealthCheckResponse
                    {
                        Component = x.Key,
                        Status = x.Value.Status.ToString(),
                        Description = x.Value.Description ?? string.Empty

                    }),
                    HealthCheckDuration = report.TotalDuration
                };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        });

        return app;
    }
    private static WebApplication ConfigureDevelopment(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DisplayRequestDuration();
            });
            app.UseHangfireDashboard();

        }

        return app;
    }
}