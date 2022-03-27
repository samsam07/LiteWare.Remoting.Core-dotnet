namespace LiteWare.Remoting.Core.Builders;

/// <summary>
/// Class that builds an instance of <see cref="RemoteService"/>.
/// </summary>
public class RemoteServiceBuilder : RemoteServiceBuilderBase<RemoteService>
{
    /// <summary>
    /// Builds an instance of <see cref="RemoteService"/> using the provided <paramref name="messagePacker"/>.
    /// </summary>
    /// <param name="messagePacker">The message packer that packs and unpacks remote messages.</param>
    /// <returns>An instance of <see cref="RemoteService"/>.</returns>
    protected override RemoteService ConfigureRemoteService(MessagePacker messagePacker) =>
        new(messagePacker);
}