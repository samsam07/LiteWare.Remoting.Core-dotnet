namespace LiteWare.Remoting.Core;

/// <summary>
/// Enumerates the different statuses of a received remote response.
/// </summary>
public enum ResponseStatus : byte
{
    /// <summary>
    /// The response was successfully processed without any errors.
    /// </summary>
    Success,

    /// <summary>
    /// The response failed to be processed and an error occurred.
    /// </summary>
    Error
}