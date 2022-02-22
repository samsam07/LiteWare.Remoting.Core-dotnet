namespace LiteWare.Remoting.Core.Exceptions;

/// <summary>
/// A general exception that is thrown when errors occur during remote operations.
/// </summary>
[Serializable]
public class RemoteException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteException"/> class.
    /// </summary>
    public RemoteException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteException" /> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for this exception.</param>
    public RemoteException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteException" /> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for this exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public RemoteException(string message, Exception inner) : base(message, inner)
    {
    }
}