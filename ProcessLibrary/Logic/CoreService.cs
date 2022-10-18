using Autofac;
using ProcessCommunication.ProcessLibrary.ViewModel;

namespace ProcessCommunication.ProcessLibrary.Logic;

/// <summary>
/// The core service class
/// </summary>
public static class CoreService
{
    private static IContainer? container;
    

    private static IContainer CreateContainer()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<DebugLogger>().As<ILogger>().InstancePerLifetimeScope();
        builder.RegisterType<MonitorServerViewModel>().AsSelf();
        return builder.Build();
    }

    /// <summary>
    /// Retrieve a service from the context.
    /// </summary>
    /// <typeparam name="T">The service to retrieve.</typeparam>
    /// <returns>The component instance that provides the service.</returns>
    public static T Resolve<T>() where T : notnull
    {
        container ??= CreateContainer();
        return container.Resolve<T>();
    }
}
