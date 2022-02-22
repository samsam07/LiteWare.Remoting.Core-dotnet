using LiteWare.Remoting.Core.Transport;

namespace LiteWare.Remoting.Core;

/// <summary>
/// Implements a mechanism that packs and unpacks remote messages.
/// </summary>
public class MessagePacker
{
    private readonly IMarshaller _marshaller;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessagePacker"/> class with the specified remote object marshaller.
    /// </summary>
    /// <param name="marshaller">A <see cref="IMarshaller"/> used for marshalling remote objects.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="marshaller"/> is <code>null</code>.</exception>
    public MessagePacker(IMarshaller marshaller)
    {
        _marshaller = marshaller ?? throw new ArgumentNullException(nameof(marshaller));
    }

    /// <summary>
    /// Packs a <see cref="RemoteCall"/> into an instance of <see cref="Message"/>.
    /// </summary>
    /// <param name="remoteCall">The remote call to pack.</param>
    /// <returns>An instance of <see cref="Message"/>.</returns>
    public Message PackRemoteCall(RemoteCall remoteCall)
    {
        byte[] remoteCallBytes = _marshaller.MarshallRemoteCall(remoteCall);
        Message message = Message.CreateCall(remoteCallBytes);

        return message;
    }

    /// <summary>
    /// Unpacks an instance of <see cref="RemoteCall"/> from the specified remote message.
    /// </summary>
    /// <param name="message">The <see cref="Message"/> to unpack.</param>
    /// <returns>An instance of <see cref="RemoteCall"/>.</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="message"/> is <code>null</code>.</exception>
    /// <exception cref="ArgumentException">
    /// The <paramref name="message"/> intent is not set to <see cref="MessageIntent.Call"/>.
    /// -or-
    /// The <paramref name="message"/> content bytes is null or of size 0.
    /// </exception>
    public RemoteCall UnpackRemoteCall(Message message)
    {
        if (message.Intent != MessageIntent.Call)
        {
            throw new ArgumentException("Wrong intent for remote call unpack.");
        }

        if (message.ContentBytes is null || message.ContentBytes.Length == 0)
        {
            throw new ArgumentException("Content bytes cannot be null or empty.");
        }

        RemoteCall remoteCall = _marshaller.UnmarshallRemoteCall(message.ContentBytes);
        return remoteCall;
    }

    /// <summary>
    /// Packs a <see cref="RemoteResponse"/> into an instance of <see cref="Message"/>.
    /// </summary>
    /// <param name="remoteResponse">The remote response to pack.</param>
    /// <returns>An instance of <see cref="Message"/>.</returns>
    public Message PackRemoteResponse(RemoteResponse remoteResponse)
    {
        byte[] remoteResponseBytes = _marshaller.MarshallRemoteResponse(remoteResponse);
        Message message = Message.CreateResponse(remoteResponseBytes);

        return message;
    }

    /// <summary>
    /// Unpacks an instance of <see cref="RemoteResponse"/> from the specified remote message.
    /// </summary>
    /// <param name="message">The <see cref="Message"/> to unpack.</param>
    /// <returns>An instance of <see cref="RemoteResponse"/>.</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="message"/> is <code>null</code>.</exception>
    /// <exception cref="ArgumentException">
    /// The <paramref name="message"/> intent is not set to <see cref="MessageIntent.Response"/>.
    /// -or-
    /// The <paramref name="message"/> content bytes is null or of size 0.
    /// </exception>
    public RemoteResponse UnpackRemoteResponse(Message message)
    {
        if (message.Intent != MessageIntent.Response)
        {
            throw new ArgumentException("Wrong intent for remote response unpack.");
        }

        if (message.ContentBytes is null || message.ContentBytes.Length == 0)
        {
            throw new ArgumentException("Content bytes cannot be null or empty.");
        }

        RemoteResponse remoteResponse = _marshaller.UnmarshallRemoteResponse(message.ContentBytes);
        return remoteResponse;
    }
}