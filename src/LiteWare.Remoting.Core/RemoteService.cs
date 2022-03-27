using System.ComponentModel;
using LiteWare.Remoting.Core.RemoteCallControllers;
using LiteWare.Remoting.Core.Transport;

namespace LiteWare.Remoting.Core;

/// <summary>
/// A service that can be configured to dispatch remote calls or handle incoming remote calls or both to and from another remote service.
/// </summary>
public class RemoteService : IRemoteServiceMediator, IRemoteCallDispatcher
{
    private readonly MessagePacker _messagePacker;

    /// <summary>
    /// Gets the service underlying <see cref="RemoteTransport"/> used to send and receive remote messages.
    /// </summary>
    public RemoteTransport? Transport { get; internal set; }

    /// <summary>
    /// Gets the remote call dispatcher of the remote service.
    /// </summary>
    public RemoteCallDispatcher? RemoteCallDispatcher { get; internal set;  }

    /// <summary>
    /// Gets the remote call handler of the remote service.
    /// </summary>
    public RemoteCallHandler? RemoteCallHandler { get; internal set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteService"/> class with the specified remote message packer.
    /// </summary>
    /// <param name="messagePacker">A <see cref="MessagePacker"/> to pack and unpack remote messages.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="messagePacker"/> is <code>null</code>.</exception>
    protected internal RemoteService(MessagePacker messagePacker)
    {
        _messagePacker = messagePacker ?? throw new ArgumentNullException(nameof(messagePacker));
    }

    void IRemoteServiceMediator.PackAndSend(RemoteCall remoteCall)
    {
        if (Transport is null)
        {
            throw new InvalidOperationException("The service transport was not properly initialized.");
        }

        Message message = _messagePacker.PackRemoteCall(remoteCall);
        Transport.Send(message);
    }

    void IRemoteServiceMediator.PackAndSend(RemoteResponse remoteResponse)
    {
        if (Transport is null)
        {
            throw new InvalidOperationException("The service transport was not properly initialized.");
        }

        Message message = _messagePacker.PackRemoteResponse(remoteResponse);
        Transport.Send(message);
    }

    private void HandleReceivedCall(Message message)
    {
        if (RemoteCallHandler is null)
        {
            return;
        }

        RemoteCall remoteCall = _messagePacker.UnpackRemoteCall(message);
        RemoteCallHandler.HandleReceivedCall(remoteCall);
    }

    private void HandleReceivedResponse(Message message)
    {
        if (RemoteCallDispatcher is null)
        {
            return;
        }

        RemoteResponse remoteResponse = _messagePacker.UnpackRemoteResponse(message);
        RemoteCallDispatcher.HandleReceivedResponse(remoteResponse);
    }

    void IRemoteServiceMediator.HandleReceivedMessage(Message message)
    {
        switch (message.Intent)
        {
            case MessageIntent.Call:
                HandleReceivedCall(message);
                break;

            case MessageIntent.Response:
                HandleReceivedResponse(message);
                break;

            default:
                throw new InvalidEnumArgumentException(nameof(message.Intent), (int)message.Intent, message.Intent.GetType());
        }
    }

    /// <summary>
    /// Calls a command with the specified parameters on a remote service and does not await for a response.
    /// </summary>
    /// <param name="command">Name of the command to invoke.</param>
    /// <param name="parameters">Parameters associated with the command.</param>
    public void Call(string command, params object?[] parameters)
    {
        if (RemoteCallDispatcher is null)
        {
            throw new InvalidOperationException("Service is not configured for remote call dispatching.");
        }

        RemoteCallDispatcher.Call(command, parameters);
    }

    /// <summary>
    /// Calls a command with the specified parameters on a remote service and awaits for a response.
    /// </summary>
    /// <param name="command">Name of the command to invoke.</param>
    /// <param name="parameters">Parameters associated with the command.</param>
    /// <returns>The object response of the called command.</returns>
    public object? Request(string command, params object?[] parameters)
    {
        if (RemoteCallDispatcher is null)
        {
            throw new InvalidOperationException("Service is not configured for remote request dispatching.");
        }

        return RemoteCallDispatcher.Request(command, parameters);
    }

    /// <summary>
    /// Asynchronously calls a command with the specified parameters on a remote service and awaits for a response.
    /// </summary>
    /// <param name="command">Name of the command to invoke.</param>
    /// <param name="parameters">Parameters associated with the command.</param>
    /// <returns>A task that represents the object response of the called command.</returns>
    public Task<object?> RequestAsync(string command, params object?[] parameters)
    {
        if (RemoteCallDispatcher is null)
        {
            throw new InvalidOperationException("Service is not configured for remote request dispatching.");
        }

        return RemoteCallDispatcher.RequestAsync(command, parameters);
    }
}