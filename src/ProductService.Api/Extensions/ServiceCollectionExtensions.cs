using Npgsql;
using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure;
using Microsoft.OpenApi.Models;
using ProductService.Common.CQRS.Behaviors;
using ProductService.Application.Behaviors;
using ProductService.Application.Features.Commands.Products;
using ProductService.Common.CQRS.UseCases.Products.CreateProduct;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using ProductService.Domain.Abtractions;
using ProductService.Infrastructure.Repositories;
using ProductService.Domain.Products;
using Asp.Versioning;

namespace ProductService.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddApiVersioning(cfg =>
        {
            cfg.DefaultApiVersion = new ApiVersion(1, 0);
            cfg.AssumeDefaultVersionWhenUnspecified = true;
            cfg.ReportApiVersions = true;
        });
        services.AddSwaggerGen(cfg =>
        {
            cfg.SwaggerDoc("v1", new OpenApiInfo { 
                                     Title = "Product API v1.0", 
                                     Version = "v1.0", 
                                     Description = "Development by TTNhan" 
                                 });
        });

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
            cfg.AddOpenBehavior(typeof(CachingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(RequestResponseLoggingBehavior<,>));
        });

        return services;
    }
    public static IServiceCollection AddRedisCacheConfiguration(this IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost";
            options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
            {
                AbortOnConnectFail = true,
                EndPoints = { options.Configuration }
            };
        });
        return services;
    }
    public static IHostBuilder AddAutoFacConfiguration(this IHostBuilder host)
    {
        host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
        {
            containerBuilder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            containerBuilder.RegisterGeneric(typeof(ReadOnlyRepository<>)).As(typeof(IReadOnlyRepository<>)).InstancePerLifetimeScope();

            var assemblies = new[] {
                typeof(IRepository<>).Assembly,
                typeof(Repository<>).Assembly
            };

            containerBuilder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();
            containerBuilder.RegisterAssemblyTypes(assemblies)
                            .Where(t => t.Name.EndsWith("Repository"))
                            .AsImplementedInterfaces()
                            .InstancePerLifetimeScope();

            //containerBuilder.RegisterAssemblyTypes(typeof(IUriService).Assembly)
            //                .Where(t => t.Name.EndsWith("Service"))
            //                .AsImplementedInterfaces()
            //                .InstancePerLifetimeScope();

            containerBuilder.RegisterType<ProductManager>().InstancePerLifetimeScope();
        });

        return host;
    }

}
