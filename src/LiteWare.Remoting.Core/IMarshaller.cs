namespace LiteWare.Remoting.Core;

/// <summary>
/// Provides a mechanism for transforming remote objects to and from an array of bytes for remote transmission.
/// </summary>
public interface IMarshaller
{
    /// <summary>
    /// Transforms the specified <see cref="RemoteCall"/> instance into an array of bytes.
    /// </summary>
    /// <param name="remoteCall">The <see cref="RemoteCall"/> to transform.</param>
    /// <returns>An array of bytes representing the remote call.</returns>
    byte[] MarshallRemoteCall(RemoteCall remoteCall);

    /// <summary>
    /// Transforms the provided array of bytes into an instance of <see cref="RemoteCall"/>.
    /// </summary>
    /// <param name="remoteCallBytes">The array of bytes to transform.</param>
    /// <returns>An instance of <see cref="RemoteCall"/>.</returns>
    RemoteCall UnmarshallRemoteCall(byte[] remoteCallBytes);

    /// <summary>
    /// Transforms the specified <see cref="RemoteResponse"/> instance into an array of bytes.
    /// </summary>
    /// <param name="remoteResponse">The <see cref="RemoteResponse"/> to transform.</param>
    /// <returns>An array of bytes representing the remote response.</returns>
    byte[] MarshallRemoteResponse(RemoteResponse remoteResponse);

    /// <summary>
    /// Transforms the provided array of bytes into an instance of <see cref="RemoteResponse"/>.
    /// </summary>
    /// <param name="remoteResponseBytes">The array of bytes to transform.</param>
    /// <returns>An instance of <see cref="RemoteResponse"/>.</returns>
    RemoteResponse UnmarshallRemoteResponse(byte[] remoteResponseBytes);
}