namespace LiteWare.Remoting.Core.RemoteCallControllers;

/// <summary>
/// Provides a mechanism to dispatch remote calls and requests to a remote service.
/// </summary>
public interface IRemoteCallDispatcher
{
    /// <summary>
    /// Calls a command with the specified parameters on a remote service and does not await for a response.
    /// </summary>
    /// <param name="command">Name of the command to invoke.</param>
    /// <param name="parameters">Parameters associated with the command.</param>
    void Call(string command, params object?[] parameters);

    /// <summary>
    /// Calls a command with the specified parameters on a remote service and awaits for a response.
    /// </summary>
    /// <param name="command">Name of the command to invoke.</param>
    /// <param name="parameters">Parameters associated with the command.</param>
    /// <returns>The object response of the called command.</returns>
    object? Request(string command, params object?[] parameters);

    /// <summary>
    /// Asynchronously calls a command with the specified parameters on a remote service and awaits for a response.
    /// </summary>
    /// <param name="command">Name of the command to invoke.</param>
    /// <param name="parameters">Parameters associated with the command.</param>
    /// <returns>A task that represents the object response of the called command.</returns>
    Task<object?> RequestAsync(string command, params object?[] parameters);
}