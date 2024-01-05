using Microsoft.Extensions.DependencyInjection;
using Order.Core.Domain.Messaging;
using System.Reflection;

namespace Order.Core.Common.Dependency
{
    public static class BasicConventionalRegistrar
    {
        public static void RegisterAssembly(this IServiceCollection services, Assembly assembly = null)
        {
            assembly = assembly ?? Assembly.GetCallingAssembly();


            services.AddServices(assembly, typeof(ITransientDependency), ServiceLifetime.Transient);
            services.AddServices(assembly, typeof(IScopedDependency), ServiceLifetime.Scoped);
            services.AddServices(assembly, typeof(ISingletonDependency), ServiceLifetime.Singleton);
            services.AddGenericServices(assembly, typeof(ICache<,>), ServiceLifetime.Transient);
            services.AddGenericServices(assembly, typeof(ICacheInvalidator<>), ServiceLifetime.Transient);
        }

        private static void AddGenericServices(this IServiceCollection services, Assembly assembly, Type dependencyInterfaceType, ServiceLifetime serviceLifetime)
        {
            var dependency = assembly.GetTypes()
                .Where(t =>
                    t.GetType().GetInterfaces()
                        .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == dependencyInterfaceType)
                    ||
                    (t.BaseType != null && t.BaseType.GetInterfaces()
                        .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == dependencyInterfaceType))
                    )
                .ToList();


            foreach (var appService in dependency)
            {
                Type[] typeParameters = (appService.BaseType ?? appService).GetGenericArguments();
                int genericArgumentCount = dependencyInterfaceType.GetGenericArguments().Length;

                services.AddService(
                    appService,
                    dependencyInterfaceType.MakeGenericType(typeParameters.Take(genericArgumentCount).ToArray()),
                    serviceLifetime
                    );
            }
        }

        private static void AddServices(this IServiceCollection services, Assembly assembly, Type dependencyInterfaceType, ServiceLifetime serviceLifetime)
        {
            var dependency = assembly.GetTypes()
                .Where(t =>
                    dependencyInterfaceType.IsAssignableFrom(t)
                    && !t.GetTypeInfo().IsGenericTypeDefinition
                    && t.IsInterface == false)
                .ToList();

            foreach (var appService in dependency)
            {
                var serviceType = appService.GetInterfaces().Where(q => q.Name.Contains(appService.Name)).FirstOrDefault();
                services.AddService(
                    appService,
                    serviceType,
                    serviceLifetime
                    );
            }
        }

        private static void AddService(this IServiceCollection services, Type? appService, Type? serviceType, ServiceLifetime serviceLifetime)
        {
            if (serviceType != null)
            {
                services.Add(new ServiceDescriptor(serviceType!, appService!, serviceLifetime));
            }
            else
            {
                services.Add(new ServiceDescriptor(appService!, appService!, serviceLifetime));
            }
        }
    }
}
