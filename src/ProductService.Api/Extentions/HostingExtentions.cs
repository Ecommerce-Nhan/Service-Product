using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProductService.Api.Middlewares;
using ProductService.Application.Exceptions;
using ProductService.Application.Mappers;
using ProductService.Common.CQRS;
using ProductService.Infrastructure;
using Serilog;

namespace ProductService.Api.Extentions;

internal static class HostingExtentions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog();
        builder.Host.AddAutoFacConfiguration();

        builder.Services.AddControllers();
        builder.Services.AddSwaggerConfiguration();
        builder.Services.AddDatabaseConfiguration(builder.Configuration);
        builder.Services.AddMediatRConfiguration();
        builder.Services.AddValidatorsFromAssembly(typeof(BaseRequest).Assembly);
        builder.Services.AddRedisCacheConfiguration();
        builder.Services.AddAutoMapper(typeof(ApplicationAutoMapperProfile).Assembly);
        builder.Services.AddHttpClient();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

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
        }

        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.UseSerilogRequestLogging();
        app.UseMiddleware<RequestDurationMiddleware>();

        return app;
    }
}