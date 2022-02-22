namespace LiteWare.Remoting.Core.Transport;

/// <summary>
/// Default implementation that serialize and deserialize <see cref="Message"/> to and from an array of bytes.
/// </summary>
public class MessageSerializer : IMessageSerializer
{
    private const int IntentSize = sizeof(MessageIntent);

    /// <summary>
    /// Serializes the provided remote message to an array of bytes.
    /// </summary>
    /// <param name="message">The message to serialize.</param>
    /// <returns>An array of bytes representing <paramref name="message"/>.</returns>
    public virtual byte[] Serialize(Message message)
    {
        ArgumentNullException.ThrowIfNull(message);

        int contentSize = message.ContentBytes.Length;
        byte[] serializedMessageBytes = new byte[IntentSize + contentSize];

        serializedMessageBytes[0] = (byte)message.Intent;
        if (contentSize > 0)
        {
            Array.Copy(message.ContentBytes, 0, serializedMessageBytes, IntentSize, contentSize);
        }

        return serializedMessageBytes;
    }

    /// <summary>
    /// Deserializes the provided array of byte into an instance of <see cref="Message"/>.
    /// </summary>
    /// <param name="messageBytes">Array of bytes representing the content of a <see cref="Message"/>.</param>
    /// <returns>An instance of <see cref="Message"/>.</returns>
    public virtual Message Deserialize(byte[] messageBytes)
    {
        ArgumentNullException.ThrowIfNull(messageBytes);
        if (messageBytes.Length == 0)
        {
            throw new ArgumentException("Byte array is shorter than expected.");
        }

        int contentSize = messageBytes.Length - IntentSize;

        byte[] contentBytes = new byte[contentSize];
        if (contentSize > 0)
        {
            Array.Copy(messageBytes, IntentSize, contentBytes, 0, contentSize);
        }

        MessageIntent intent = (MessageIntent)messageBytes[0];
        Message message = new(intent, contentBytes);

        return message;
    }
}