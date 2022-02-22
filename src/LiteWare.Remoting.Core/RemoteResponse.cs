namespace LiteWare.Remoting.Core;

/// <summary>
/// Represents a response to a remote call request.
/// </summary>
public record RemoteResponse(ResponseStatus Status, Guid Reference, object? Value, string? ErrorMessage)
{
    /// <summary>
    /// Creates a new <see cref="RemoteResponse"/> with a success status.
    /// </summary>
    /// <param name="reference">The reference of the response.</param>
    /// <param name="value">The response value.</param>
    /// <returns>A <see cref="RemoteResponse"/>.</returns>
    public static RemoteResponse CreateSuccess(Guid reference, object? value) =>
        new(ResponseStatus.Success, reference, value, null);

    /// <summary>
    /// Creates a new <see cref="RemoteResponse"/> with an error status.
    /// </summary>
    /// <param name="reference">The reference of the response.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A <see cref="RemoteResponse"/>.</returns>
    public static RemoteResponse CreateError(Guid reference, string errorMessage) =>
        new(ResponseStatus.Error, reference, null, errorMessage);

    /// <summary>
    /// Gets or sets the response status.
    /// </summary>
    public ResponseStatus Status { get; set; } = Status;

    /// <summary>
    /// Gets or sets the reference of the remote call that initiated the response.
    /// </summary>
    public Guid Reference { get; set; } = Reference;

    /// <summary>
    /// Gets or sets an error message if an error occurred during a remote call.
    /// </summary>
    public string? ErrorMessage { get; set; } = ErrorMessage;

    /// <summary>
    /// Gets or sets the response value.
    /// </summary>
    public object? Value { get; set; } = Value;
}