namespace LiteWare.Remoting.Core;

/// <summary>
/// Provides a mechanism to invoke commands.
/// </summary>
public interface ICommandInvoker
{
    /// <summary>
    /// Invoke the specified command with the specified parameters.
    /// </summary>
    /// <param name="command">The name of the command to invoke.</param>
    /// <param name="parameters">The parameters of the command to invoke.</param>
    /// <returns>An <see cref="object"/> representing the command invoke response.</returns>
    object? InvokeCommand(string command, params object?[] parameters);
}