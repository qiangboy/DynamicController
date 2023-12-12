using Autofac;
using System.Reflection;

namespace DependencyInjection;

public static class ContainerBuilderExtensions
{
    public static void RegisterDependency(this ContainerBuilder builder, params Assembly[] assemblies)
    {
        // 扫描程序集中所有继承自ITransientDependency的类，并以Transient生命周期方式进行注册
        builder.RegisterAssemblyTypes(assemblies)
            .Where(type => typeof(ITransientDependency).IsAssignableFrom(type) && type is { IsClass: true, IsAbstract: false })
            .AsImplementedInterfaces()
            .InstancePerDependency();

        // 扫描程序集中所有继承自ISingletonDependency的类，并以Singleton生命周期方式进行注册
        builder.RegisterAssemblyTypes(assemblies)
            .Where(type => typeof(ISingletonDependency).IsAssignableFrom(type) && type is { IsClass: true, IsAbstract: false })
            .AsImplementedInterfaces()
            .SingleInstance();

        // 扫描程序集中所有继承自IScopedDependency的类，并以Scoped生命周期方式进行注册
        builder.RegisterAssemblyTypes(assemblies)
            .Where(type => typeof(IScopedDependency).IsAssignableFrom(type) && type is { IsClass: true, IsAbstract: false })
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}