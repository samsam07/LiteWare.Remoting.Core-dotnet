using LiteWare.Remoting.Core.RemoteCallControllers;

namespace LiteWare.Remoting.Core.Builders;

/// <summary>
/// Class that builds an instance of <see cref="RemoteCallDispatcher"/>.
/// </summary>
public class RemoteCallDispatcherBuilder
{
    /// <summary>
    /// Gets or sets the request timeout of the remote call dispatcher to build.
    /// Set to <c>null</c> to use the default one.
    /// </summary>
    public TimeSpan? RequestTimeout { get; set; }

    /// <summary>
    /// Builds an instance of <see cref="RemoteCallDispatcher"/> using the configurations set by the current builder.
    /// </summary>
    /// <param name="remoteServiceMediator">A <see cref="IRemoteServiceMediator"/> used to notify the need for packing and sending of remote calls.</param>
    /// <returns>An instance of <see cref="RemoteCallDispatcher"/>.</returns>
    public RemoteCallDispatcher Build(IRemoteServiceMediator remoteServiceMediator)
    {
        RemoteCallDispatcher remoteCallDispatcher = new(remoteServiceMediator);
        if (RequestTimeout is not null)
        {
            remoteCallDispatcher.RequestTimeout = RequestTimeout.Value;
        }

        return remoteCallDispatcher;
    }
}