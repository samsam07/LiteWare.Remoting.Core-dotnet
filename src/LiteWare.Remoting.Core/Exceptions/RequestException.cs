namespace LiteWare.Remoting.Core.Exceptions;

/// <summary>
/// A general exception that is thrown when errors occur during remote request.
/// </summary>
[Serializable]
public class RequestException : RemoteException
{
    /// <summary>
    /// Gets the remote call request.
    /// </summary>
    public RemoteCall? RemoteCall { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestException"/> class.
    /// </summary>
    public RequestException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestException" /> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for this exception.</param>
    public RequestException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestException" /> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for this exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public RequestException(string message, Exception inner) : base(message, inner)
    {
    }
}