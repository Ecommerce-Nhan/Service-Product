using Amazon.S3;
using Asp.Versioning;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using ProductService.Api.Authorize;
using ProductService.Application.Features.Products.Commands.Create;
using ProductService.Application.Permissions;
using ProductService.Application.Services.S3;
using ProductService.Domain.Exceptions.Products;
using ProductService.Domain.Products;
using ProductService.Infrastructure;
using ProductService.Infrastructure.Repositories;
using SharedLibrary.CQRS.Behaviors;
using SharedLibrary.Exceptions;
using SharedLibrary.Repositories.Abtractions;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

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
        });

        return host;
    }
    public static IServiceCollection AddHandleException(this IServiceCollection services)
    {
        services.AddExceptionHandler<ProductExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }
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
    public static IServiceCollection AddHangfireConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddHangfire(x => x
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_110)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UsePostgreSqlStorage(a =>
                                              a.UseNpgsqlConnection(connectionString),
                                              new PostgreSqlStorageOptions
                                              {
                                                  QueuePollInterval = TimeSpan.FromSeconds(30),
                                                  UseNativeDatabaseTransactions = false,
                                                  DistributedLockTimeout = TimeSpan.FromMinutes(10),
                                                  InvisibilityTimeout = TimeSpan.FromMinutes(10),
                                              })
                        );
        services.AddHangfireServer();

        return services;
    }
    public static IServiceCollection AddJWTConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o =>
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("IxrAjDoa2FqElO7IhrSrUJELhUckePEPVpaePlS_Xaw"));
            o.RequireHttpsMetadata = false;
            o.SaveToken = true; 
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,

                //ValidIssuer = "https://localhost:5001/",
                //ValidAudience = "b865bfc2-9966-4309-93be-f0dcd2d7c59b",
                IssuerSigningKey = key,
            };
        });
        services.AddAuthorization(options =>
        {
          
        });
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        return services;
    }
    public static IServiceCollection AddRedis(this IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "RedisCache";
            options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
            {
                AbortOnConnectFail = true,
                EndPoints = { options.Configuration }
            };
        });

        return services;
    }
}
