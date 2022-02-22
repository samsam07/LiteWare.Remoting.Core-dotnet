using System.ComponentModel;

namespace LiteWare.Remoting.Core.RemoteCallControllers;

/// <summary>
/// Implements a mechanism to handle remote calls received from a remote service.
/// </summary>
public class RemoteCallHandler
{
    private readonly IRemoteServiceMediator _remoteServiceMediator;
    private readonly ICommandInvoker _commandInvoker;

    /// <summary>
    /// Gets or sets a value indicating whether the reception of remote calls should be handled asynchronously.
    /// </summary>
    public bool HandleReceivedCallsAsynchronously { get; set; } = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteCallHandler"/> class with the specified remote service mediator.
    /// </summary>
    /// <param name="remoteServiceMediator">A <see cref="IRemoteServiceMediator"/> used to notify the need for packing and sending of remote responses.</param>
    /// <param name="commandInvoker">A <see cref="ICommandInvoker"/> used to invoke commands.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="remoteServiceMediator"/> is <code>null</code>.
    /// -or-
    /// The <paramref name="commandInvoker"/> is <code>null</code>.
    /// </exception>
    public RemoteCallHandler(IRemoteServiceMediator remoteServiceMediator, ICommandInvoker commandInvoker)
    {
        _remoteServiceMediator = remoteServiceMediator ?? throw new ArgumentNullException(nameof(remoteServiceMediator));
        _commandInvoker = commandInvoker ?? throw new ArgumentNullException(nameof(commandInvoker));
    }

    private object? InvokeCommand(string command, object?[] parameters)
    {
        object? invokeResult = _commandInvoker.InvokeCommand(command, parameters);
        if (invokeResult is not Task task)
        {
            return invokeResult;
        }

        task.Wait();
        if (invokeResult.GetType().IsGenericType) // Is Task<?>
        {
            return invokeResult
                .GetType()
                .GetProperty("Result")
                ?.GetValue(invokeResult);
        }

        return null;
    }

    private void HandleCall(string command, params object?[] parameters) =>
        InvokeCommand(command, parameters);

    private void HandleRequest(Guid reference, string command, params object?[] parameters)
    {
        RemoteResponse response;
        try
        {
            object? responseValue = InvokeCommand(command, parameters);
            response = RemoteResponse.CreateSuccess(reference, responseValue);
        }
        catch (Exception exception)
        {
            response = RemoteResponse.CreateError(reference, exception.Message);
        }

        _remoteServiceMediator.PackAndSend(response);
    }

    /// <summary>
    /// Handles the reception of a <see cref="RemoteCall"/> received from a remote service.
    /// </summary>
    /// <param name="remoteCall">The call request received from a remote service.</param>
    public void HandleReceivedCall(RemoteCall remoteCall)
    {
        Action handleReceivedCallAction = remoteCall.CallType switch
        {
            CallType.FireAndForget => (() => HandleCall(remoteCall.Command, remoteCall.Parameters)),
            CallType.AwaitCallback => (() => HandleRequest(remoteCall.Reference, remoteCall.Command, remoteCall.Parameters)),
            _ => throw new InvalidEnumArgumentException(nameof(remoteCall.CallType), (int)remoteCall.CallType, remoteCall.CallType.GetType())
        };

        if (HandleReceivedCallsAsynchronously)
        {
            Task.Run(handleReceivedCallAction);
        }
        else
        {
            handleReceivedCallAction();
        }
    }
}