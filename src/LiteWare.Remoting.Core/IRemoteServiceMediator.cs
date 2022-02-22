using LiteWare.Remoting.Core.Transport;

#if DEBUG
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("LiteWare.Remoting.Core.UnitTests")]
#endif
namespace LiteWare.Remoting.Core;

/// <summary>
/// Represents a mediator that inks and allows the transfer of data between the different subsystems of a remote service.
/// </summary>
public interface IRemoteServiceMediator
{
    /// <summary>
    /// Packs and sends a remote call to a remote service.
    /// </summary>
    /// <param name="remoteCall">The <see cref="RemoteCall"/> to pack and send.</param>
    protected internal void PackAndSend(RemoteCall remoteCall);

    /// <summary>
    /// Packs and sends a remote response to a remote service.
    /// </summary>
    /// <param name="remoteResponse">The <see cref="RemoteResponse"/> to pack and send.</param>
    protected internal void PackAndSend(RemoteResponse remoteResponse);

    /// <summary>
    /// Handles the reception of a <see cref="Message"/> received from a remote service. 
    /// </summary>
    /// <param name="message">The message received from a remote service.</param>
    protected internal void HandleReceivedMessage(Message message);
}