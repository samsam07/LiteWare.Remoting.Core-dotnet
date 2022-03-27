using LiteWare.Remoting.Core.Builders;

namespace LiteWare.Remoting.Core.Fluent.RemoteServiceDefinitions;

internal sealed class RemoteServiceDefinition : IRemoteServiceDefinition,
    IWithRemoteCallDispatcher,
        IOptionalWithRequestTimeout,
    IWithRemoteCallHandler,
        IWithCommandInvoker,
        IOptionalAsSyncAsync,
    IWithNetworking,
    IWithMarshaller,
    IWithCreate
{
    private readonly RemoteServiceBuilder _remoteServiceBuilder = new();

    public IAlternativeWithRemoteCallHandlerOrWithNetworking WithRemoteCallDispatcher(RemoteCallDispatcherBuilder remoteCallDispatcherBuilder)
    {
        _remoteServiceBuilder.RemoteCallDispatcherBuilder = remoteCallDispatcherBuilder;
        return this;
    }

    public IOptionalWithRequestTimeout WithRemoteCallDispatcher()
    {
        _remoteServiceBuilder.RemoteCallDispatcherBuilder = new RemoteCallDispatcherBuilder();
        return this;
    }

    public IAlternativeWithRemoteCallHandlerOrWithNetworking HavingRequestTimeout(TimeSpan timeout)
    {
        _remoteServiceBuilder.RemoteCallDispatcherBuilder!.RequestTimeout = timeout;
        return this;
    }

    public IAlternativeWithRemoteCallHandlerOrWithNetworking HavingRequestTimeout(double milliseconds)
    {
        _remoteServiceBuilder.RemoteCallDispatcherBuilder!.RequestTimeout = TimeSpan.FromMilliseconds(milliseconds);
        return this;
    }

    public IWithNetworking WithRemoteCallHandler(RemoteCallHandlerBuilder remoteCallHandlerBuilder)
    {
        _remoteServiceBuilder.RemoteCallHandlerBuilder = remoteCallHandlerBuilder;
        return this;
    }

    public IWithCommandInvoker WithRemoteCallHandler()
    {
        _remoteServiceBuilder.RemoteCallHandlerBuilder = new RemoteCallHandlerBuilder();
        return this;
    }

    public IOptionalAsSyncAsync UsingCommandInvoker(ICommandInvoker commandInvoker)
    {
        _remoteServiceBuilder.RemoteCallHandlerBuilder!.CommandInvoker = commandInvoker;
        return this;
    }

    public IWithNetworking Synchronously()
    {
        _remoteServiceBuilder.RemoteCallHandlerBuilder!.HandleAsynchronously = false;
        return this;
    }

    public IWithNetworking Asynchronously()
    {
        _remoteServiceBuilder.RemoteCallHandlerBuilder!.HandleAsynchronously = true;
        return this;
    }

    public IWithMarshaller OnNetwork(RemoteNetworkBuilder remoteNetworkBuilder)
    {
        _remoteServiceBuilder.RemoteNetworkBuilder = remoteNetworkBuilder;
        return this;
    }

    public IWithCreate WithMarshaller(IMarshaller marshaller)
    {
        _remoteServiceBuilder.Marshaller = marshaller;
        return this;
    }

    public Core.RemoteService Create() =>
        _remoteServiceBuilder.Build();
}