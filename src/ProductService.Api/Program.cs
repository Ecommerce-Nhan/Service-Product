using Serilog;
using FluentValidation;
using ProductService.Api.Extentions;
using ProductService.Application.Exceptions;
using ProductService.Application.Mappers;
using ProductService.Common.CQRS.Models.Requests;
using ProductService.Domain.Abtractions;
using ProductService.Domain.Products;
using ProductService.Infrastructure.Repositories;
using ProductService.Infrastructure.Repositories.Products;
using ProductService.Infrastructure;
using Microsoft.EntityFrameworkCore;

try
{
    Log.Logger = new LoggerConfiguration().WriteTo.Console()
                                                  .CreateLogger();
    Log.Information("starting server.");

    var builder = WebApplication.CreateBuilder(args);

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

    var app = builder.Build();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        await context.Database.MigrateAsync();
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
