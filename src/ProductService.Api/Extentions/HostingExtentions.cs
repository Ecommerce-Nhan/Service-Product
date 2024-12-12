using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Exceptions;
using ProductService.Application.Mappers;
using ProductService.Common.CQRS;
using ProductService.Domain.Abtractions;
using ProductService.Domain.Products;
using ProductService.Infrastructure;
using ProductService.Infrastructure.Repositories;
using ProductService.Infrastructure.Repositories.Products;
using Serilog;

namespace ProductService.Api.Extentions;

internal static class HostingExtentions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration.WriteTo.Console();
            loggerConfiguration.ReadFrom.Configuration(context.Configuration);
        });

        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>));
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IProductReadOnlyRepository, ProductReadOnlyRepository>();

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

        return app;
    }
}