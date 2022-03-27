using LiteWare.Remoting.Core.Builders;
using LiteWare.Remoting.Core.Transport;

namespace LiteWare.Remoting.Core.Fluent.RemoteServiceDefinitions.Template;

/// <summary>
/// The unlinked stage of the fluent remote service definition allowing to specify a remote network.
/// </summary>
/// <typeparam name="TNext">The type of the next stage of the definition.</typeparam>
public interface IWithNetworking<TNext> where TNext : class
{
    /// <summary>
    /// Specifies a <see cref="RemoteNetworkBuilder"/> that will build a <see cref="RemoteNetwork"/> for the the remote service to create.
    /// </summary>
    /// <param name="remoteNetworkBuilder">A <see cref="RemoteNetworkBuilder"/> that will build a <see cref="RemoteNetwork"/>.</param>
    /// <returns>The next stage of the definition.</returns>
    TNext OnNetwork(RemoteNetworkBuilder remoteNetworkBuilder);
}