using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using MediatR.NotificationPublishers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Common.Behaviours;
using Product.Application.Common.Errors;
using Product.Core.Common.Dependency;
using System.Reflection;

namespace Product.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSingleton<ProblemDetailsFactory, ApiProblemDetailsFactory>();
            services.AddHttpContextAccessor();

            services.AddMappings();

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                config.NotificationPublisher = new ForeachAwaitPublisher();//For Publish Notifications In Parallel
            });
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PerformanceMonitorBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CacheInvalidationBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CacheBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(IntegrationEventBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehaviour<,>));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("redis");
            });

            //services.AddScoped(typeof(ICache<LoginQuery, ErrorOr<LoginResponse>>), typeof(LoginQueryCache));
            //services.AddTransient(typeof(ICacheInvalidator<RegisterCommand>), typeof(UpdateUserGetUsersCacheInvalidator));
            services.RegisterAssembly();

            return services;
        }

        private static IServiceCollection AddMappings(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
            return services;
        }
    }
}
