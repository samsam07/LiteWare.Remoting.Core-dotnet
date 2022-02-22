namespace LiteWare.Remoting.Core.Transport;

/// <summary>
/// Represents data exchange between remote services.
/// </summary>
public record Message(MessageIntent Intent, byte[] ContentBytes)
{
    /// <summary>
    /// Creates a new remote service call message.
    /// </summary>
    /// <param name="contentBytes">An array of bytes representing the message content.</param>
    /// <returns>An instance of <see cref="Message"/>.</returns>
    public static Message CreateCall(byte[] contentBytes) =>
        new(MessageIntent.Call, contentBytes);

    /// <summary>
    /// Creates a new remote service response message.
    /// </summary>
    /// <param name="contentBytes">An array of bytes representing the message content.</param>
    /// <returns>An instance of <see cref="Message"/>.</returns>
    public static Message CreateResponse(byte[] contentBytes) =>
        new(MessageIntent.Response, contentBytes);

    /// <summary>
    /// Gets or sets the message intent.
    /// </summary>
    public MessageIntent Intent { get; set; } = Intent;

    /// <summary>
    /// Gets or sets an array of bytes representing the message content.
    /// </summary>
    public byte[] ContentBytes { get; set; } = ContentBytes;
}