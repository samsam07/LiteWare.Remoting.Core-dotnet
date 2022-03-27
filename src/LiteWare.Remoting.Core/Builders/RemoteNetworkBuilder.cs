using LiteWare.Remoting.Core.Transport;

namespace LiteWare.Remoting.Core.Builders;

/// <summary>
/// Base class that builds an instance of <see cref="RemoteNetwork"/>.
/// </summary>
public abstract class RemoteNetworkBuilder
{
    /// <summary>
    /// Gets or sets the <see cref="IMessageSerializer"/> used by the remote network to build.
    /// </summary>
    public IMessageSerializer? MessageSerializer { get; set; }

    /// <summary>
    /// Builds an instance of <see cref="RemoteNetwork"/> using the configurations set by the current builder.
    /// </summary>
    /// <param name="remoteServiceMediator">A <see cref="IRemoteServiceMediator"/> used to notify remote message reception.</param>
    /// <returns>An instance of <see cref="RemoteNetwork"/>.</returns>
    public abstract RemoteNetwork Build(IRemoteServiceMediator remoteServiceMediator);
}