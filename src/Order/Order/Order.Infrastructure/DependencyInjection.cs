using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Order.Application.Common.Abstractions;
using Order.Core.Common.Dependency;
using Order.Core.Domain.Messaging.Events;
using Order.Core.EventBusRabbitMQ;
using Order.Core.EventBusRabbitMQ.Settings;
using Order.Core.IntegrationEventLogEF;
using Order.Core.IntegrationEventLogEF.Services;
using Order.Infrastructure.Persistence;
using Order.Infrastructure.Persistence.IntegrationEvents;
using Order.Infrastructure.Persistence.Settings;
using RabbitMQ.Client;
using System.Data.Common;
using System.Reflection;

namespace Order.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {

            services.Configure<MessageBrokerSettings>(configuration.GetSection(MessageBrokerSettings.SettingsKey));
            services.Configure<ConnectionStringSettings>(configuration.GetSection(ConnectionStringSettings.SettingsKey));

            services
                .AddDatabase(configuration)
                .AddEventBus(configuration);


            //services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            //services.AddScoped<IUserRepository, UserRepository>();
            services.RegisterAssembly();

            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services, ConfigurationManager configuration)
        {
            services
                //.AddInMemoryDbContexts(configuration)
                .AddSqlServerContexts(configuration)
                ;

            services.AddScoped<IDbContext>(factory => factory.GetRequiredService<OrderDbContext>());
            //services.AddScoped<IUnitOfWork>(factory => factory.GetRequiredService<OrderDbContext>());

            return services;
        }


        private static IServiceCollection AddInMemoryDbContexts(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbContext<OrderDbContext>(ServiceLifetime.Scoped);
            services.AddDbContext<IntegrationEventLogContext>(ServiceLifetime.Scoped);

            return services;
        }


        private static IServiceCollection AddSqlServerContexts(this IServiceCollection services, ConfigurationManager configuration)
        {
            var conn = configuration.GetConnectionString("sql");

            services.AddDbContext<OrderDbContext>((Action<DbContextOptionsBuilder>?)GetSqlServerOptions(conn), ServiceLifetime.Scoped);
            services.AddDbContext<IntegrationEventLogContext>((Action<DbContextOptionsBuilder>?)GetSqlServerOptions(conn), ServiceLifetime.Scoped);

            return services;
        }

        private static Action<DbContextOptionsBuilder> GetSqlServerOptions(string? conn)
        {
            return options =>
            {
                options.UseSqlServer(conn,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                        //sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            };
        }

        private static IServiceCollection AddEventBus(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
                sp => (DbConnection c) => new IntegrationEventLogService(c
                    //, Assembly.GetAssembly(typeof(Contracts.ApiRoutes.ApiRoutes))!.FullName
                    //, Assembly.GetAssembly(typeof(Application.DependencyInjection))!.FullName
                    )
                );

            services.AddTransient<IOrderIntegrationEventService, OrderIntegrationEventService>();


            var settings = new MessageBrokerSettings();
            configuration.Bind(MessageBrokerSettings.SettingsKey, settings);
            services.AddSingleton(Options.Create(settings));


            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = settings.HostName,
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(settings.UserName))
                {
                    factory.UserName = settings.UserName;
                }

                if (!string.IsNullOrEmpty(settings.Password))
                {
                    factory.Password = settings.Password;
                }

                if (settings.Port > 0)
                {
                    factory.Port = settings.Port;
                }

                var retryCount = 3;
                if (settings.RetryCount > 0)
                {
                    retryCount = settings.RetryCount;
                }


                //var factory = new RabbitMQ.Client.ConnectionFactory
                //{
                //    Uri = new Uri("amqp://guest:guest@rabbitmq:5672/")
                //};

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });

            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var subscriptionClientName = settings.QueueName;
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();

                var retryCount = 5;
                if (settings.RetryCount > 0)
                {
                    retryCount = settings.RetryCount;
                }
                var mediator = sp.GetRequiredService<IMediator>();

                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, mediator, subscriptionClientName, retryCount);
            });

            return services;
        }
    }
}