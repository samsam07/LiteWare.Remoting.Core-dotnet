using LiteWare.Remoting.Core.Builders;
using LiteWare.Remoting.Core.Transport;

namespace LiteWare.Remoting.Core.Fluent.RemoteServiceDefinitions.Template;

/// <summary>
/// The unlinked stage of the fluent remote service definition allowing to specify a remote transport.
/// </summary>
/// <typeparam name="TNext">The type of the next stage of the definition.</typeparam>
public interface IWithTransport<TNext> where TNext : class
{
    /// <summary>
    /// Specifies a <see cref="RemoteTransportBuilder"/> that will build a <see cref="RemoteTransport"/> for the the remote service to create.
    /// </summary>
    /// <param name="remoteTransportBuilder">A <see cref="RemoteTransportBuilder"/> that will build a <see cref="RemoteTransport"/>.</param>
    /// <returns>The next stage of the definition.</returns>
    TNext OnTransport(RemoteTransportBuilder remoteTransportBuilder);
}