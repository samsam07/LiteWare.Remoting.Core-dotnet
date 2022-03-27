using LiteWare.Remoting.Core.RemoteCallControllers;

namespace LiteWare.Remoting.Core.Builders;

/// <summary>
/// Class that builds an instance of <see cref="RemoteCallHandler"/>.
/// </summary>
public class RemoteCallHandlerBuilder
{
    /// <summary>
    /// Gets or sets a <see cref="ICommandInvoker"/> used by the remote call handler to build.
    /// </summary>
    public ICommandInvoker? CommandInvoker { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the reception of remote calls should be handled asynchronously.
    /// Set to <c>null</c> to use the default one.
    /// </summary>
    public bool? HandleAsynchronously { get; set; }

    /// <summary>
    /// Builds an instance of <see cref="RemoteCallHandler"/> using the configurations set by the current builder.
    /// </summary>
    /// <param name="remoteServiceMediator">A <see cref="IRemoteServiceMediator"/> used to notify the need for packing and sending of remote responses.</param>
    /// <returns>An instance of <see cref="RemoteCallHandler"/>.</returns>
    public RemoteCallHandler Build(IRemoteServiceMediator remoteServiceMediator)
    {
        ValidateState();

        RemoteCallHandler remoteCallHandler = new(remoteServiceMediator, CommandInvoker!);
        if (HandleAsynchronously.HasValue)
        {
            remoteCallHandler.HandleReceivedCallsAsynchronously = HandleAsynchronously.Value;
        }

        return remoteCallHandler;
    }

    private void ValidateState()
    {
        if (CommandInvoker is null)
        {
            throw new InvalidOperationException($"{nameof(CommandInvoker)} was not initialized.");
        }
    }
}