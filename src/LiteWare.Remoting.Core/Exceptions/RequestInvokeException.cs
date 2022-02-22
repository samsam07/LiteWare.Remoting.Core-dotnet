namespace LiteWare.Remoting.Core.Exceptions;

/// <summary>
/// Exception that is thrown when a request to invoke a remote command fails on the remote endpoint.
/// </summary>
[Serializable]
public class RequestInvokeException : RequestException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequestInvokeException"/> class.
    /// </summary>
    public RequestInvokeException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestInvokeException" /> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for this exception.</param>
    public RequestInvokeException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestInvokeException" /> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for this exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public RequestInvokeException(string message, Exception inner) : base(message, inner)
    {
    }
}
