namespace LiteWare.Remoting.Core.Fluent.RemoteServiceDefinitions.Template;

/// <summary>
/// The unlinked final stage of the fluent remote service definition that creates an instance of <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The instance type which will be created at the end of this stage of the definition.</typeparam>
public interface IWithCreate<T>
{
    /// <summary>
    /// Creates an instance of <typeparamref name="T"/> with all the specifications defined earlier.
    /// </summary>
    /// <returns>An instance of <typeparamref name="T"/>.</returns>
    T Create();
}