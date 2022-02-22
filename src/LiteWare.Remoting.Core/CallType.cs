namespace LiteWare.Remoting.Core;

/// <summary>
/// Enumerates the different types of remote calls.
/// </summary>
public enum CallType : byte
{
    /// <summary>
    /// The call is a one way call and no response is awaited.
    /// </summary>
    FireAndForget,

    /// <summary>
    /// The call is a request and a response is awaited.
    /// </summary>
    AwaitCallback
}