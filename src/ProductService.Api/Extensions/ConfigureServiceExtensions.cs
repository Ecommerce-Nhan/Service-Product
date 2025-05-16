using CategoryService.Application.Handlers;
using FluentValidation;
using ProductService.Application.Handlers;
using ProductService.Application.Mappers;
using ProductService.Infrastructure;
using Serilog;

namespace ProductService.Api.Extensions;

internal static partial class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();
        // Host Configuration
        builder.Host.UseSerilog();
        builder.Host.AddAutoFacConfiguration();

        // Default Configuration
        builder.Services.AddHttpClient();
        builder.Services.AddControllers();
        builder.Services.AddAutoMapper(typeof(ProductAutoMapperProfile).Assembly);
        builder.Services.AddValidatorsFromAssembly(typeof(ProductAutoMapperProfile).Assembly);

        // Custom Configuration
        builder.Services.AddExceptionHandler<ProductExceptionHandler>();
        builder.Services.AddExceptionHandler<CategoryExceptionHandler>();
        builder.Services.AddSwaggerConfiguration();
        builder.Services.AddMediatRConfiguration();
        builder.Services.AddAWSConfiguration(builder.Configuration);
        builder.Services.AddDatabaseConfiguration(builder.Configuration);
        builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();

        return builder.Build();
    }
}