namespace LiteWare.Remoting.Core.Exceptions;

/// <summary>
/// Exception that is thrown when a remote request fails to receive a response withing an allocated time.
/// </summary>
[Serializable]
public class RequestTimeoutException : RequestException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequestTimeoutException"/> class.
    /// </summary>
    public RequestTimeoutException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestTimeoutException" /> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for this exception.</param>
    public RequestTimeoutException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestTimeoutException" /> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for this exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public RequestTimeoutException(string message, Exception inner) : base(message, inner)
    {
    }
}