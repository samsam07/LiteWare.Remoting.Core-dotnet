namespace LiteWare.Remoting.Core.Fluent.RemoteServiceDefinitions.Template;

/// <summary>
/// The unlinked stage of the fluent remote service definition allowing to specify a marshaller.
/// </summary>
/// <typeparam name="TNext">The type of the next stage of the definition.</typeparam>
public interface IWithMarshaller<TNext> where TNext : class
{
    /// <summary>
    /// Specifies a <see cref="IMarshaller"/> used by the remote service to create.
    /// </summary>
    /// <param name="marshaller">A marshaller for transforming remote objects to and from an array of bytes for remote transmission.</param>
    /// <returns>The next stage of the definition.</returns>
    TNext WithMarshaller(IMarshaller marshaller);
}