using LiteWare.Remoting.Core.Builders;

namespace LiteWare.Remoting.Core.Fluent.RemoteServiceDefinitions;

internal sealed class RemoteServiceDefinition : IRemoteServiceDefinition,
    IWithRemoteCallDispatcher,
        IOptionalWithRequestTimeout,
    IWithRemoteCallHandler,
        IWithCommandInvoker,
        IOptionalAsSyncAsync,
    IWithTransport,
    IWithMarshaller,
    IWithCreate
{
    private readonly RemoteServiceBuilder _remoteServiceBuilder = new();

    public IAlternativeWithRemoteCallHandlerOrWithTransport WithRemoteCallDispatcher(RemoteCallDispatcherBuilder remoteCallDispatcherBuilder)
    {
        _remoteServiceBuilder.RemoteCallDispatcherBuilder = remoteCallDispatcherBuilder;
        return this;
    }

    public IOptionalWithRequestTimeout WithRemoteCallDispatcher()
    {
        _remoteServiceBuilder.RemoteCallDispatcherBuilder = new RemoteCallDispatcherBuilder();
        return this;
    }

    public IAlternativeWithRemoteCallHandlerOrWithTransport HavingRequestTimeout(TimeSpan timeout)
    {
        _remoteServiceBuilder.RemoteCallDispatcherBuilder!.RequestTimeout = timeout;
        return this;
    }

    public IAlternativeWithRemoteCallHandlerOrWithTransport HavingRequestTimeout(double milliseconds)
    {
        _remoteServiceBuilder.RemoteCallDispatcherBuilder!.RequestTimeout = TimeSpan.FromMilliseconds(milliseconds);
        return this;
    }

    public IWithTransport WithRemoteCallHandler(RemoteCallHandlerBuilder remoteCallHandlerBuilder)
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

    public IWithTransport Synchronously()
    {
        _remoteServiceBuilder.RemoteCallHandlerBuilder!.HandleAsynchronously = false;
        return this;
    }

    public IWithTransport Asynchronously()
    {
        _remoteServiceBuilder.RemoteCallHandlerBuilder!.HandleAsynchronously = true;
        return this;
    }

    public IWithMarshaller OnTransport(RemoteTransportBuilder remoteTransportBuilder)
    {
        _remoteServiceBuilder.RemoteTransportBuilder = remoteTransportBuilder;
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