namespace LiteWare.Remoting.Core.Transport;

/// <summary>
/// Provides a mechanism to serialize and deserialize <see cref="Message"/> to and from an array of bytes.
/// </summary>
public interface IMessageSerializer
{
    /// <summary>
    /// Serializes the provided remote message to an array of bytes.
    /// </summary>
    /// <param name="remoteMessage">The message to serialize.</param>
    /// <returns>An array of bytes representing <paramref name="remoteMessage"/>.</returns>
    byte[] Serialize(Message remoteMessage);

    /// <summary>
    /// Deserializes the provided array of byte into an instance of <see cref="Message"/>.
    /// </summary>
    /// <param name="remoteMessageBytes">Array of bytes representing the content of a <see cref="Message"/>.</param>
    /// <returns>An instance of <see cref="Message"/>.</returns>
    Message Deserialize(byte[] remoteMessageBytes);
}