using LiteWare.Remoting.Core.Builders;
using LiteWare.Remoting.Core.RemoteCallControllers;
using LiteWare.Remoting.Core.Transport;

namespace LiteWare.Remoting.Core.Fluent;

/// <summary>
/// The entry point for the fluent remote service definition.
/// </summary>
public interface IFluentRemoteService : IWithMarshaller { }

/// <summary>
/// The stage of the fluent remote service definition allowing to specify a marshaller.
/// </summary>
public interface IWithMarshaller
{
    /// <summary>
    /// Specifies a <see cref="IMarshaller"/> used by the remote service to create.
    /// </summary>
    /// <param name="marshaller">A marshaller for transforming remote objects to and from an array of bytes for remote transmission.</param>
    /// <returns>The next stage of the definition.</returns>
    IWithNetworking WithMarshaller(IMarshaller marshaller);
}

/// <summary>
/// The stage of the fluent remote service definition allowing to specify a remote network.
/// </summary>
public interface IWithNetworking
{
    /// <summary>
    /// Specifies a <see cref="RemoteNetworkBuilder"/> that will build a <see cref="RemoteNetwork"/> for the the remote service to create.
    /// </summary>
    /// <param name="remoteNetworkBuilder">A <see cref="RemoteNetworkBuilder"/> that will build a <see cref="RemoteNetwork"/>.</param>
    /// <returns>The next stage of the definition.</returns>
    IWithRemoteCallDispatcher ForNetwork(RemoteNetworkBuilder remoteNetworkBuilder);
}

/// <summary>
/// The stage of the fluent remote service definition allowing the configuration of a remote call dispatcher.
/// </summary>
public interface IWithRemoteCallDispatcher
{
    /// <summary>
    /// Specifies that a remote call dispatcher will not be configured for the the remote service to create.
    /// </summary>
    /// <returns>The next stage of the definition.</returns>
    IWithRemoteCallHandling DisableRemoteCallDispatching();

    /// <summary>
    /// Specifies a <see cref="RemoteCallDispatcherBuilder"/> that will build a <see cref="RemoteCallDispatcher"/> for the the remote service to create.
    /// </summary>
    /// <param name="remoteCallDispatcherBuilder">A <see cref="RemoteCallDispatcherBuilder"/> that will build a <see cref="RemoteCallDispatcher"/>.</param>
    /// <returns>The next stage of the definition.</returns>
    IWithRemoteCallHandling EnableRemoteCallDispatching(RemoteCallDispatcherBuilder remoteCallDispatcherBuilder);

    /// <summary>
    /// Specifies that a remote call dispatcher will be configured for the the remote service to create.
    /// </summary>
    /// <returns>The optional sub-stage for configuring the remote call dispatcher.</returns>
    IOptionalWithTimeout EnableRemoteCallDispatching();
}

/// <summary>
/// The optional sub-stage of the remote call dispatcher configuration stage allowing to specify a request timeout.
/// </summary>
public interface IOptionalWithTimeout : IWithRemoteCallHandling
{
    /// <summary>
    /// Specifies the request timeout of the remote call dispatcher.
    /// </summary>
    /// <param name="timeout">The timespan to wait before the request times out.</param>
    /// <returns>The next stage of the definition.</returns>
    public IWithRemoteCallHandling WithTimeout(TimeSpan timeout);

    /// <summary>
    /// Specifies the request timeout of the remote call dispatcher.
    /// </summary>
    /// <param name="milliseconds">The time in milliseconds to wait before the request times out.</param>
    /// <returns>The next stage of the definition.</returns>
    public IWithRemoteCallHandling WithTimeout(double milliseconds);
}

/// <summary>
/// The stage of the fluent remote service definition allowing the configuration of a remote call handler.
/// </summary>
public interface IWithRemoteCallHandling
{
    /// <summary>
    /// Specifies that a remote call handler will not be configured for the the remote service to create.
    /// </summary>
    /// <returns>The final stage of the definition.</returns>
    IWithCreate DisableRemoteCallHandling();

    /// <summary>
    /// Specifies a <see cref="RemoteCallHandlerBuilder"/> that will build a <see cref="RemoteCallHandler"/> for the the remote service to create.
    /// </summary>
    /// <param name="remoteCallHandlerBuilder">A <see cref="RemoteCallHandlerBuilder"/> that will build a <see cref="RemoteCallHandler"/>.</param>
    /// <returns>The final stage of the definition.</returns>
    IWithCreate EnableRemoteCallHandling(RemoteCallHandlerBuilder remoteCallHandlerBuilder);

    /// <summary>
    /// Specifies that a remote call handler will be configured for the the remote service to create.
    /// </summary>
    /// <returns>The sub-stage for configuring the remote call handler.</returns>
    IWithCommandInvoker EnableRemoteCallHandling();
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
    IOptionalAsSyncOrAsync UsingCommandInvoker(ICommandInvoker commandInvoker);
}

/// <summary>
/// The optional sub-stage of the remote call handler configuration stage allowing to specify whether remote call handling is done synchronously or asynchronously.
/// </summary>
public interface IOptionalAsSyncOrAsync
{
    /// <summary>
    /// Specifies that remote call handling is done synchronously.
    /// </summary>
    /// <returns>The final stage of the definition.</returns>
    IWithCreate Synchronously();

    /// <summary>
    /// Specifies that remote call handling is done asynchronously.
    /// </summary>
    /// <returns>The final stage of the definition.</returns>
    IWithCreate Asynchronously();
}

/// <summary>
/// The final stage of the fluent remote service definition that creates an instance of <see cref="RemoteService"/>.
/// </summary>
public interface IWithCreate
{
    /// <summary>
    /// Creates an instance of <see cref="RemoteService"/> with all the specifications defined earlier.
    /// </summary>
    /// <returns>An instance of <see cref="RemoteService"/>.</returns>
    RemoteService Create();
}