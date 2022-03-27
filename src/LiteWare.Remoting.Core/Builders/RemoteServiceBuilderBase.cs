using LiteWare.Remoting.Core.RemoteCallControllers;
using LiteWare.Remoting.Core.Transport;

namespace LiteWare.Remoting.Core.Builders;

/// <summary>
/// Base class that builds an instance or a derived instance of <see cref="RemoteService"/>.
/// </summary>
/// <typeparam name="TRemoteService">The <see cref="RemoteService"/> type or derived type that will be build by the builder.</typeparam>
public abstract class RemoteServiceBuilderBase<TRemoteService> where TRemoteService : RemoteService
{
    /// <summary>
    /// Gets or sets the builder that builds an instance of <see cref="RemoteCallDispatcher"/> used by the remote service to build.
    /// Set to <c>null</c> to omit a remote call dispatcher.
    /// </summary>
    public RemoteCallDispatcherBuilder? RemoteCallDispatcherBuilder { get; set; }

    /// <summary>
    /// Gets or sets the builder that builds an instance of <see cref="RemoteCallHandler"/> used by the remote service to build.
    /// Set to <c>null</c> to omit a remote call handler.
    /// </summary>
    public RemoteCallHandlerBuilder? RemoteCallHandlerBuilder { get; set; }

    /// <summary>
    /// Gets or sets the builder that builds an instance of <see cref="RemoteNetwork"/> used by the remote service to build.
    /// </summary>
    public RemoteNetworkBuilder? RemoteNetworkBuilder { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="IMarshaller"/> used by the remote service to build.
    /// </summary>
    public IMarshaller? Marshaller { get; set; }

    /// <summary>
    /// Builds an instance of <typeparamref name="TRemoteService"/> using the configurations set by the current builder.
    /// </summary>
    /// <returns>An instance of <typeparamref name="TRemoteService"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// <see cref="Marshaller"/> is null.
    /// -or-
    /// Both <see cref="RemoteCallDispatcherBuilder"/> and <see cref="RemoteCallHandlerBuilder"/> are null.
    /// -or-
    /// <see cref="RemoteNetworkBuilder"/> is null.
    /// </exception>
    public TRemoteService Build()
    {
        if (Marshaller is null)
        {
            throw new InvalidOperationException("Marshaller was not initialized.");
        }

        MessagePacker messagePacker = new(Marshaller!);
        TRemoteService remoteService = ConfigureRemoteService(messagePacker);

        if (RemoteCallDispatcherBuilder is null && RemoteCallHandlerBuilder is null)
        {
            throw new InvalidOperationException("Cannot configure a remote service where both remote call dispatching and remote call handling are disabled.");
        }

        if (RemoteNetworkBuilder is null)
        {
            throw new InvalidOperationException("Remote network was not initialized.");
        }

        remoteService.RemoteCallDispatcher = RemoteCallDispatcherBuilder?.Build(remoteService);
        remoteService.RemoteCallHandler = RemoteCallHandlerBuilder?.Build(remoteService);
        remoteService.Network = RemoteNetworkBuilder!.Build(remoteService);

        return remoteService;
    }

    /// <summary>
    /// When implemented in a derived class, configures and builds an instance of <typeparamref name="TRemoteService"/> using the provided <paramref name="messagePacker"/>.
    /// </summary>
    /// <param name="messagePacker">The message packer that packs and unpacks remote messages.</param>
    /// <returns>An instance of <typeparamref name="TRemoteService"/>.</returns>
    protected abstract TRemoteService ConfigureRemoteService(MessagePacker messagePacker);
}