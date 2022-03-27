using LiteWare.Remoting.Core.Builders;
using LiteWare.Remoting.Core.Fluent.RemoteServiceDefinitions.Template;
using LiteWare.Remoting.Core.RemoteCallControllers;

namespace LiteWare.Remoting.Core.Fluent.RemoteServiceDefinitions;

/// <summary>
/// The entry point for the fluent remote service definition.
/// </summary>
public interface IRemoteServiceDefinition : IWithRemoteCallDispatcher, IWithRemoteCallHandler { }

/// <summary>
/// The stage of the fluent remote service definition allowing the configuration of a remote call dispatcher.
/// </summary>
public interface IWithRemoteCallDispatcher
{
    /// <summary>
    /// Specifies a <see cref="RemoteCallDispatcherBuilder"/> that will build a <see cref="RemoteCallDispatcher"/> for the the remote service to create.
    /// </summary>
    /// <param name="remoteCallDispatcherBuilder">A <see cref="RemoteCallDispatcherBuilder"/> that will build a <see cref="RemoteCallDispatcher"/>.</param>
    /// <returns>The next stage of the definition.</returns>
    IAlternativeWithRemoteCallHandlerOrWithNetworking WithRemoteCallDispatcher(RemoteCallDispatcherBuilder remoteCallDispatcherBuilder);

    /// <summary>
    /// Specifies that a remote call dispatcher will be configured for the the remote service to create.
    /// </summary>
    /// <returns>The optional sub-stage for configuring the remote call dispatcher.</returns>
    IOptionalWithRequestTimeout WithRemoteCallDispatcher();
}

/// <summary>
/// The stage of the fluent remote service definition allowing the configuration of either the remote call handler or to specify a remote network.
/// </summary>
public interface IAlternativeWithRemoteCallHandlerOrWithNetworking : IWithRemoteCallHandler, IWithNetworking { }

/// <summary>
/// The optional sub-stage of the remote call dispatcher configuration stage allowing to specify a request timeout.
/// </summary>
public interface IOptionalWithRequestTimeout : IAlternativeWithRemoteCallHandlerOrWithNetworking
{
    /// <summary>
    /// Specifies the request timeout of the remote call dispatcher.
    /// </summary>
    /// <param name="timeout">The timespan to wait before the request times out.</param>
    /// <returns>The next stage of the definition.</returns>
    IAlternativeWithRemoteCallHandlerOrWithNetworking HavingRequestTimeout(TimeSpan timeout);

    /// <summary>
    /// Specifies the request timeout of the remote call dispatcher.
    /// </summary>
    /// <param name="milliseconds">The time in milliseconds to wait before the request times out.</param>
    /// <returns>The next stage of the definition.</returns>
    IAlternativeWithRemoteCallHandlerOrWithNetworking HavingRequestTimeout(double milliseconds);
}

/// <summary>
/// The stage of the fluent remote service definition allowing the configuration of a remote call handler.
/// </summary>
public interface IWithRemoteCallHandler
{
    /// <summary>
    /// Specifies a <see cref="RemoteCallHandlerBuilder"/> that will build a <see cref="RemoteCallHandler"/> for the the remote service to create.
    /// </summary>
    /// <param name="remoteCallHandlerBuilder">A <see cref="RemoteCallHandlerBuilder"/> that will build a <see cref="RemoteCallHandler"/>.</param>
    /// <returns>The next stage of the definition.</returns>
    IWithNetworking WithRemoteCallHandler(RemoteCallHandlerBuilder remoteCallHandlerBuilder);

    /// <summary>
    /// Specifies that a remote call handler will be configured for the the remote service to create.
    /// </summary>
    /// <returns>The sub-stage for configuring the remote call handler.</returns>
    IWithCommandInvoker WithRemoteCallHandler();
}

/// <summary>
/// The sub-stage of the remote call handler configuration stage allowing to specify a command invoker.
/// </summary>
public interface IWithCommandInvoker
{
    /// <summary>
    /// Specifies a <see cref="ICommandInvoker"/> used by the remote call handler.
    /// </summary>
    /// <param name="commandInvoker">A <see cref="ICommandInvoker"/> used to invoke commands.</param>
    /// <returns>The next optional sub-stage for configuring the remote call handler.</returns>
    IOptionalAsSyncAsync UsingCommandInvoker(ICommandInvoker commandInvoker);
}

/// <summary>
/// The optional sub-stage of the remote call handler configuration stage allowing to specify whether remote call handling is done synchronously or asynchronously.
/// </summary>
public interface IOptionalAsSyncAsync : IWithNetworking
{
    /// <summary>
    /// Specifies that remote call handling is done synchronously.
    /// </summary>
    /// <returns>The next stage of the definition.</returns>
    IWithNetworking Synchronously();

    /// <summary>
    /// Specifies that remote call handling is done asynchronously.
    /// </summary>
    /// <returns>The next stage of the definition.</returns>
    IWithNetworking Asynchronously();
}

/// <summary>
/// The stage of the fluent remote service definition allowing to specify a remote network.
/// </summary>
public interface IWithNetworking : IWithNetworking<IWithMarshaller> { }

/// <summary>
/// The stage of the fluent remote service definition allowing to specify a marshaller.
/// </summary>
public interface IWithMarshaller : IWithMarshaller<IWithCreate> { }

/// <summary>
/// The final stage of the fluent remote service definition that creates an instance of <see cref="RemoteService"/>.
/// </summary>
public interface IWithCreate : IWithCreate<Core.RemoteService> { }