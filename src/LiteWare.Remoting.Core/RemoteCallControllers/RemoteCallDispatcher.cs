using System.ComponentModel;
using LiteWare.Remoting.Core.Exceptions;

namespace LiteWare.Remoting.Core.RemoteCallControllers;

/// <summary>
/// Implements a mechanism to dispatch remote calls and requests to a remote service.
/// </summary>
public class RemoteCallDispatcher : IRemoteCallDispatcher
{
    private const int DefaultRequestTimeout = 30;

    private readonly IRemoteServiceMediator _remoteServiceMediator;
    private readonly Dictionary<Guid, RemoteResponseAwaiter> _requestPool = new();

    /// <summary>
    /// Gets or sets the timespan to wait before the request times out.
    /// </summary>
    public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(DefaultRequestTimeout);

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteCallDispatcher"/> class with the specified remote service mediator.
    /// </summary>
    /// <param name="remoteServiceMediator">A <see cref="IRemoteServiceMediator"/> used to notify the need for packing and sending of remote calls.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="remoteServiceMediator"/> is <code>null</code>.</exception>
    public RemoteCallDispatcher(IRemoteServiceMediator remoteServiceMediator)
    {
        _remoteServiceMediator = remoteServiceMediator ?? throw new ArgumentNullException(nameof(remoteServiceMediator));
    }

    /// <summary>
    /// Calls a command with the specified parameters on a remote service and does not await for a response.
    /// </summary>
    /// <param name="command">Name of the command to invoke.</param>
    /// <param name="parameters">Parameters associated with the command.</param>
    public void Call(string command, params object?[] parameters)
    {
        RemoteCall remoteCall = RemoteCall.CreateOneWayCall(command, parameters);
        _remoteServiceMediator.PackAndSend(remoteCall);
    }

    /// <summary>
    /// Calls a command with the specified parameters on a remote service and awaits for a response.
    /// </summary>
    /// <param name="command">Name of the command to invoke.</param>
    /// <param name="parameters">Parameters associated with the command.</param>
    /// <returns>The object response of the called command.</returns>
    public object? Request(string command, params object?[] parameters)
    {
        Task<object?> task = RequestAsync(command, parameters);
        task.Wait();

        return task.Result;
    }

    /// <summary>
    /// Asynchronously calls a command with the specified parameters on a remote service and awaits for a response.
    /// </summary>
    /// <param name="command">Name of the command to invoke.</param>
    /// <param name="parameters">Parameters associated with the command.</param>
    /// <returns>A task that represents the object response of the called command.</returns>
    public async Task<object?> RequestAsync(string command, params object?[] parameters)
    {
        Guid reference = Guid.NewGuid();
        RemoteCall remoteCall = RemoteCall.CreateRequest(reference, command, parameters);

        RemoteResponse? response;
        using (RemoteResponseAwaiter responseAwaiter = new(RequestTimeout))
        {
            try
            {
                _requestPool.Add(reference, responseAwaiter);
                _remoteServiceMediator.PackAndSend(remoteCall);

                response = await responseAwaiter.WaitForResponse();
            }
            finally
            {
                _requestPool.Remove(reference);
            }
        }

        if (response is null)
        {
            throw new RequestTimeoutException {  RemoteCall = remoteCall };
        }

        return response.Status switch
        {
            ResponseStatus.Success => response.Value,
            ResponseStatus.Error => throw new RequestInvokeException(response.ErrorMessage!) { RemoteCall = remoteCall },
            _ => throw new InvalidEnumArgumentException(nameof(response.Status), (int)response.Status, response.Status.GetType())
        };
    }

    /// <summary>
    /// Handles the reception of a <see cref="RemoteResponse"/> received from a remote service.
    /// </summary>
    /// <param name="response">The response received from a remote service.</param>
    public void HandleReceivedResponse(RemoteResponse response)
    {
        if (_requestPool.ContainsKey(response.Reference))
        {
            _requestPool[response.Reference].SignalResponse(response);
        }
    }
}