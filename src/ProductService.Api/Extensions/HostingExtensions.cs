using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Mappers;
using SharedLibrary.CQRS;
using ProductService.Infrastructure;
using Serilog;
using SharedLibrary.Extentions;
using Hangfire;

namespace ProductService.Api.Extensions;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // Host Configuration
        builder.Host.UseSerilog();
        builder.Host.AddAutoFacConfiguration();

        // Default Configuration
        builder.Services.AddControllers();
        builder.Services.AddHttpClient();
        builder.Services.AddValidatorsFromAssembly(typeof(BaseRequest).Assembly);
        builder.Services.AddAutoMapper(typeof(ProductAutoMapperProfile).Assembly);

        // Custom Configuration
        builder.Services.AddSwaggerConfiguration();
        builder.Services.AddDatabaseConfiguration(builder.Configuration);
        builder.Services.AddMediatRConfiguration();
        builder.Services.AddRedisCacheConfiguration();
        builder.Services.AddHandleException();
        builder.Services.AddAWSConfiguration(builder.Configuration);
        builder.Services.AddHangfireConfiguration(builder.Configuration);

        return builder.Build();
    }
    public static WebApplication ConfigurePipeline(this WebApplication app, WebApplicationBuilder builder)
    {

        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DisplayRequestDuration();
            });
            app.UseHangfireDashboard();
        }

        app.UseExceptionHandler("/error");
        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();
        app.MapControllers();

        return app;
    }
}