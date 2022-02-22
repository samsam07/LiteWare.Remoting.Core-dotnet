namespace LiteWare.Remoting.Core;

/// <summary>
/// Represents a call to a remote service.
/// </summary>
public record RemoteCall(CallType CallType, Guid Reference, string Command, object?[] Parameters)
{
    /// <summary>
    /// Creates a new <see cref="RemoteCall"/> record as a fire and forget call.
    /// </summary>
    /// <param name="command">The remote command to invoke.</param>
    /// <param name="parameters">The parameters of the remote command.</param>
    /// <returns>A <see cref="RemoteCall"/>.</returns>
    public static RemoteCall CreateOneWayCall(string command, object?[] parameters) =>
        new(CallType.FireAndForget, Guid.Empty, command, parameters);

    /// <summary>
    /// Creates a new <see cref="RemoteCall"/> record as a request that will generate a response on a remote service.
    /// </summary>
    /// <param name="reference">The reference of the call.</param>
    /// <param name="command">The remote command to invoke.</param>
    /// <param name="parameters">The parameters of the remote command.</param>
    /// <returns>A <see cref="RemoteCall"/>.</returns>
    public static RemoteCall CreateRequest(Guid reference, string command, object?[] parameters) =>
        new(CallType.AwaitCallback, reference, command, parameters);

    /// <summary>
    /// Gets or sets the call type.
    /// </summary>
    public CallType CallType { get; set; } = CallType;

    /// <summary>
    /// Gets or sets the reference of the call.
    /// </summary>
    public Guid Reference { get; set; } = Reference;

    /// <summary>
    /// Gets or sets the name of a remote command to invoke.
    /// </summary>
    public string Command { get; set; } = Command;

    /// <summary>
    /// Get or sets the parameters of the remote command.
    /// </summary>
    public object?[] Parameters { get; set; } = Parameters;
}