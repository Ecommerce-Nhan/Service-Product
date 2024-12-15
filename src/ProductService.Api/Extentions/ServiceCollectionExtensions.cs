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

namespace ProductService.Api.Extentions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product API", Version = "v1.0.0" });
            c.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
        });

        return services;
    }
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);
        services.AddDbContext<AppDbContext>(o => o.UseNpgsql(connectionStringBuilder.ConnectionString));

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
            cfg.AddOpenBehavior(typeof(RequestResponseLoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
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

        });

        return host;
    }
}
