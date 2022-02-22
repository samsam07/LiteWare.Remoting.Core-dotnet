using LiteWare.Remoting.Core.Builders;

namespace LiteWare.Remoting.Core.Fluent;

internal sealed class FluentRemoteService : IFluentRemoteService,
    IWithMarshaller,
    IWithNetworking,
    IWithRemoteCallDispatcher,
        IOptionalWithTimeout,
    IWithRemoteCallHandling,
        IWithCommandInvoker,
        IOptionalAsSyncOrAsync,
    IWithCreate
{
    public static IFluentRemoteService Configure() =>
        new FluentRemoteService();

    private IMarshaller? _marshaller;
    private RemoteNetworkBuilder? _remoteNetworkBuilder;
    private RemoteCallDispatcherBuilder? _remoteCallDispatcherBuilder;
    private RemoteCallHandlerBuilder? _remoteCallHandlerBuilder;

    private FluentRemoteService() { }

    public IWithNetworking WithMarshaller(IMarshaller marshaller)
    {
        _marshaller = marshaller;
        return this;
    }

    public IWithRemoteCallDispatcher ForNetwork(RemoteNetworkBuilder remoteNetworkBuilder)
    {
        _remoteNetworkBuilder = remoteNetworkBuilder;
        return this;
    }

    public IWithRemoteCallHandling DisableRemoteCallDispatching()
    {
        _remoteCallDispatcherBuilder = null;
        return this;
    }

    public IWithRemoteCallHandling EnableRemoteCallDispatching(RemoteCallDispatcherBuilder remoteCallDispatcherBuilder)
    {
        _remoteCallDispatcherBuilder = remoteCallDispatcherBuilder;
        return this;
    }

    public IOptionalWithTimeout EnableRemoteCallDispatching()
    {
        _remoteCallDispatcherBuilder = new RemoteCallDispatcherBuilder();
        return this;
    }

    public IWithRemoteCallHandling WithTimeout(TimeSpan timeout)
    {
        _remoteCallDispatcherBuilder!.RequestTimeout = timeout;
        return this;
    }

    public IWithRemoteCallHandling WithTimeout(double milliseconds)
    {
        _remoteCallDispatcherBuilder!.RequestTimeout = TimeSpan.FromMilliseconds(milliseconds);
        return this;
    }

    public IWithCreate DisableRemoteCallHandling()
    {
        _remoteCallHandlerBuilder = null;
        return this;
    }

    public IWithCreate EnableRemoteCallHandling(RemoteCallHandlerBuilder remoteCallHandlerBuilder)
    {
        _remoteCallHandlerBuilder = remoteCallHandlerBuilder;
        return this;
    }

    public IWithCommandInvoker EnableRemoteCallHandling()
    {
        _remoteCallHandlerBuilder = new RemoteCallHandlerBuilder();
        return this;
    }

    public IOptionalAsSyncOrAsync UsingCommandInvoker(ICommandInvoker commandInvoker)
    {
        _remoteCallHandlerBuilder!.CommandInvoker = commandInvoker;
        return this;
    }

    public IWithCreate Synchronously()
    {
        _remoteCallHandlerBuilder!.HandleAsynchronously = false;
        return this;
    }

    public IWithCreate Asynchronously()
    {
        _remoteCallHandlerBuilder!.HandleAsynchronously = true;
        return this;
    }

    public RemoteService Create()
    {
        ValidateState();
        
        MessagePacker messagePacker = new(_marshaller!);

        RemoteService remoteService = new(messagePacker);
        remoteService.Network = _remoteNetworkBuilder!.Build(remoteService);
        remoteService.RemoteCallDispatcher = _remoteCallDispatcherBuilder?.Build(remoteService);
        remoteService.RemoteCallHandler = _remoteCallHandlerBuilder?.Build(remoteService);

        return remoteService;
    }

    private void ValidateState()
    {
        if (_marshaller is null)
        {
            throw new InvalidOperationException("Marshaller was not initialized.");
        }

        if (_remoteNetworkBuilder is null)
        {
            throw new InvalidOperationException("Remote network was not initialized.");
        }

        if (_remoteCallDispatcherBuilder is null && _remoteCallHandlerBuilder is null)
        {
            throw new InvalidOperationException("Cannot configure a remote service where both remote call dispatching and remote call handling are disabled.");
        }
    }
}