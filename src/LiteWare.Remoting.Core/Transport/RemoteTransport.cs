namespace LiteWare.Remoting.Core.Transport;

/// <summary>
/// Provide a base implementation for transmission and reception of remote messages between remote services.
/// </summary>
public abstract class RemoteTransport
{
    private readonly IRemoteServiceMediator _remoteServiceMediator;
    private readonly IMessageSerializer _messageSerializer;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteTransport"/> class with the specified remote service mediator and message serializer.
    /// </summary>
    /// <param name="remoteServiceMediator">A <see cref="IRemoteServiceMediator"/> used to notify remote message reception.</param>
    /// <param name="messageSerializer">A <see cref="IMessageSerializer"/> used to serialize and deserialize remote messages.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="remoteServiceMediator"/> is <code>null</code>.
    /// -or-
    /// The <paramref name="messageSerializer"/> is <code>null</code>.
    /// </exception>
    protected RemoteTransport(IRemoteServiceMediator remoteServiceMediator, IMessageSerializer messageSerializer)
    {
        _remoteServiceMediator = remoteServiceMediator ?? throw new ArgumentNullException(nameof(remoteServiceMediator));
        _messageSerializer = messageSerializer ?? throw new ArgumentNullException(nameof(messageSerializer));
    }

    /// <summary>
    /// When implemented in a derived class, sends the specified array of bytes representing a <see cref="Message"/> to a remote service.
    /// </summary>
    /// <param name="messageBytes">Array of bytes representing a <see cref="Message"/>.</param>
    protected abstract void SendBytes(byte[] messageBytes);

    /// <summary>
    /// Sends a <see cref="Message"/> to a remote service.
    /// </summary>
    /// <param name="message">The <see cref="Message"/> to send.</param>
    public void Send(Message message)
    {
        byte[] messageBytes = _messageSerializer.Serialize(message);
        SendBytes(messageBytes);
    }

    /// <summary>
    /// Handles the specified array of bytes representing a <see cref="Message"/> received from a remote service.
    /// </summary>
    /// <param name="messageBytes">An array of bytes representing a <see cref="Message"/>.</param>
    protected void HandleReceivedMessageBytes(byte[] messageBytes)
    {
        Message message = _messageSerializer.Deserialize(messageBytes);
        _remoteServiceMediator.HandleReceivedMessage(message);
    }
}