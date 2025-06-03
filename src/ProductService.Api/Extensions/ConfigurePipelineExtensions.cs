using Hangfire;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductService.Infrastructure;
using Serilog;
using SharedLibrary.Dtos.HealthChecks;

namespace ProductService.Api.Extensions;

internal static partial class HostingExtensions
{
    public static WebApplication ConfigurePipeline(this WebApplication app, WebApplicationBuilder builder)
    {
        app.MapControllers();
        app.ConfigureDevelopment();
        app.CheckHealthy();
        app.UseExceptionHandler("/error");
        app.UseSerilogRequestLogging();
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
    private static WebApplication CheckHealthy(this WebApplication app)
    {
        app.UseHealthChecks("/api/v1/product/health", new HealthCheckOptions
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
        if (!app.Environment.IsProduction())
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