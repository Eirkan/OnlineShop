using Customer.Application.Common.Abstractions;
using Customer.Core.Common.Dependency;
using Customer.Core.Domain.Messaging.Events;
using Customer.Core.EventBusRabbitMQ;
using Customer.Core.EventBusRabbitMQ.Settings;
using Customer.Core.IntegrationEventLogEF;
using Customer.Core.IntegrationEventLogEF.Services;
using Customer.Infrastructure.Persistence;
using Customer.Infrastructure.Persistence.IntegrationEvents;
using Customer.Infrastructure.Persistence.Settings;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace Customer.Infrastructure
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

            services.RegisterAssembly();

            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services, ConfigurationManager configuration)
        {
            services
                //.AddInMemoryDbContexts(configuration)
                .AddSqlServerContexts(configuration)
                ;

            services.AddScoped<IDbContext>(factory => factory.GetRequiredService<CustomerDbContext>());

            return services;
        }


        private static IServiceCollection AddInMemoryDbContexts(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbContext<CustomerDbContext>(ServiceLifetime.Scoped);
            services.AddDbContext<IntegrationEventLogContext>(ServiceLifetime.Scoped);

            return services;
        }


        private static IServiceCollection AddSqlServerContexts(this IServiceCollection services, ConfigurationManager configuration)
        {
            var conn = configuration.GetConnectionString("sql");

            services.AddDbContext<CustomerDbContext>((Action<DbContextOptionsBuilder>?)GetSqlServerOptions(conn), ServiceLifetime.Scoped);
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

            services.AddTransient<ICustomerIntegrationEventService, CustomerIntegrationEventService>();


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