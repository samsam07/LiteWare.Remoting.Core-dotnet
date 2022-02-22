#if DEBUG
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("LiteWare.ObjectInvokers.UnitTests")]
#endif
namespace LiteWare.Remoting.Core.RemoteCallControllers;

internal sealed class RemoteResponseAwaiter : IDisposable
{
    private readonly ManualResetEvent _blockingEvent = new(false);
    private RemoteResponse? _response;

    public TimeSpan Timeout { get; set; }

    public RemoteResponseAwaiter(TimeSpan timeout)
    {
        Timeout = timeout;
    }

    public void SignalResponse(RemoteResponse response)
    {
        _response = response;
        _blockingEvent.Set();
    }

    public async Task<RemoteResponse?> WaitForResponse()
    {
        _blockingEvent.Reset();

        bool gotResponse = await Task.Run(() => _blockingEvent.WaitOne(Timeout));
        RemoteResponse? response = gotResponse ? _response : null;

        return response;
    }

    #region IDisposable Support

    private bool _disposedValue;

    public void Dispose()
    {
        if (!_disposedValue)
        {
            _blockingEvent.Dispose();
            _disposedValue = true;
        }
    }

    #endregion IDisposable Support
}
