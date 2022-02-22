namespace LiteWare.Remoting.Core.Transport;

/// <summary>
/// Enumerates the different intentions of a sent or received remote message.
/// </summary>
public enum MessageIntent : byte
{
    /// <summary>
    /// Message is a call to a remote service.
    /// </summary>
    Call,

    /// <summary>
    /// Message is a response to a remote service call.
    /// </summary>
    Response
}