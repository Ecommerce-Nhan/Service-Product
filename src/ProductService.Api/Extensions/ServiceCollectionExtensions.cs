using Amazon.S3;
using Asp.Versioning;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CategoryService.Domain.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql;
using Orchestration.ServiceDefaults.Behaviors;
using ProductService.Application.Features.Products.Commands.Create;
using ProductService.Application.Services.S3;
using ProductService.Domain.Products;
using ProductService.Domain.Variants;
using ProductService.Infrastructure;
using ProductService.Infrastructure.Repositories;
using SharedLibrary.Repositories.Abtractions;

namespace ProductService.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IHostBuilder AddAutoFacConfiguration(this IHostBuilder host)
    {
        host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
        {
            containerBuilder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            containerBuilder.RegisterGeneric(typeof(ReadOnlyRepository<>)).As(typeof(IReadOnlyRepository<>)).InstancePerLifetimeScope();

            var assemblies = new[] {
                typeof(IProductRepository).Assembly,
                typeof(IRepository<>).Assembly,
                typeof(Repository<>).Assembly
            };

            containerBuilder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();
            containerBuilder.RegisterAssemblyTypes(assemblies)
                            .Where(t => t.Name.EndsWith("Repository"))
                            .AsImplementedInterfaces()
                            .InstancePerLifetimeScope();

            containerBuilder.RegisterAssemblyTypes(typeof(IS3Service).Assembly)
                            .Where(t => t.Name.EndsWith("Service"))
                            .AsImplementedInterfaces()
                            .InstancePerLifetimeScope();

            containerBuilder.RegisterType<ProductManager>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<VariantManager>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<CategoryManager>().InstancePerLifetimeScope();
        });

        return host;
    }

    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddApiVersioning(cfg =>
        {
            cfg.DefaultApiVersion = new ApiVersion(1, 0);
            cfg.AssumeDefaultVersionWhenUnspecified = true;
            cfg.ReportApiVersions = true;
            cfg.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("x-api-version"),
                new MediaTypeApiVersionReader("x-api-version"));
            cfg.UnsupportedApiVersionStatusCode = StatusCodes.Status400BadRequest;
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Product API v1",
                Version = "v1",
                Description = "Development by TTNhan"
            });

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter 'Bearer {token}'",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            options.AddSecurityDefinition("Bearer", securityScheme);

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    securityScheme,
                    Array.Empty<string>()
                }
            };

            options.AddSecurityRequirement(securityRequirement);
        });

        return services;
    }
    public static IServiceCollection AddMediatRConfiguration(this IServiceCollection services)
    {
        var assemblies = new[]
        {
            typeof(CreateProductCommand).Assembly,
            typeof(CreateProductCommandHandler).Assembly,
        };
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assemblies);
            //cfg.AddOpenBehavior(typeof(CachingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            //cfg.AddOpenBehavior(typeof(RequestResponseLoggingBehavior<,>));
        });

        return services;
    }
    public static IServiceCollection AddAWSConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        services.AddAWSService<IAmazonS3>();

        return services;
    }
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);
        services.AddDbContext<AppDbContext>(o => o.UseNpgsql(connectionStringBuilder.ConnectionString));
        services.AddDbContext<AppReadOnlyDbContext>(o => o.UseNpgsql(connectionStringBuilder.ConnectionString));

        return services;
    }
}
