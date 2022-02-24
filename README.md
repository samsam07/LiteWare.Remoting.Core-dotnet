# LiteWare.Remoting.Core for .NET

[![Nuget](https://img.shields.io/nuget/v/LiteWare.Remoting.Core)](https://www.nuget.org/packages/LiteWare.Remoting.Core)
[![License](https://img.shields.io/github/license/samsam07/LiteWare.Remoting.Core-dotnet)](https://github.com/samsam07/LiteWare.Remoting.Core-dotnet/blob/master/LICENSE)

LiteWare's core implementation for distributed systems. It provides the barebone communication framework that handles the flow of data between connected distributed systems. Built with flexibility and extensibility in mind, the core framework was designed to provide many abstractions to allow customizations in implementing libraries. For instance, you can create a custom transport (HTTP, IPC or whatever protocol) to transport payload between remote services.

## Infrastructure overview

The framework uses the `RemoteService` class for all communications. It is the main service of the framework and is broken down into 4 sub-systems:

![RemoteServie overview](docs/images/RemoteService-Overview.png)

1. The transport (a.k.a `RemoteNetwork`)  
   It is responsible for serialization and sending/receiving of payloads over a network.

2. The service mediator  
   It is responsible for payload marshalling and its distribution across the other sub-systems.

3. The optionally configured Remote call Dispatcher (a.k.a `RemoteCallDispatcher`)  
   It is responsible for invoking remote calls on other remote services.  
   This sub-system is optional and can be omitted during configuration. This will prevent the service from invoking remote calls.

4. The optionally configured Remote call Handler (a.k.a `RemoteCallHandler`)  
   It is responsible for the handling of incoming remote calls and to create response to these incoming calls.  
   This sub-system is optional and can be omitted during configuration. This will prevent the service from handling remote calls.

## Implementing the core library

Any library implementing LiteWare.Remoting.Core should implement the following:

### 1. Custom marshaller (`IMarshaller`)

The `RemoteService` requires an implementation of `IMarshaller` to convert remote calls and requests to and from an array of bytes.

### 2. Custom transport (`RemoteNetwork`, `IMessageSerializer` and `RemoteNetworkBuilder`)

The transport is represented by the abstract `RemoteNetwork` class and is responsible for the exchange of payloads between remote services. This class should be extended and specialized to use a specific communication protocol.

The `RemoteNetwork` is also responsible for the serialization and deserialization of payloads and thus, requires that an implementation of `IMessageSerializer` is provided during its construction. `IMessageSerializer` is different from `IMarshaller`. The latter serializes remote objects (`RemoteCall` and `RemoteResponse`) while the former serializes framed payloads (`Message`) containing remote objects.

Implementations of the `RemoteNetwork` class should prevent direct initialization through constructors. A derivation of the `RemoteNetworkBuilder` class should instead be used to build new instances as this allows for a proper configuration of the network in a remote service.

### 3. Custom command invoking

An implementation of `ICommandInvoker` must be provided if a `RemoteService` is configured to handle incoming remote calls. The `ICommandInvoker` is tasked with interpreting the remote call to generate a response.

## Using `RemoteService`

`RemoteService` is the main class that allows remoting on distributed systems. It can be configured in 3 ways:

1. It can be configured to handle incoming remote calls  
   This will automatically create and send a response when needed and can be done synchronously or asynchronously.

2. It can also be configured to dispatch remote calls. Once done, the following code can be executed:

   ``` cs
   remoteService.Call("IncrementCounter"); // Make a one-way call to a remote service
   int? count = (int?)remoteService.Request("Count"); // Make a request and synchronously await for a response
   object? value = await remoteService.RequestAsync("CalculateSum", 1, 2, 3); // Make a request and asynchronously await for a response
   ```

3. Or it can be configured to do both at the same time.

Configuring a `RemoteService` is done using fluent language as shown below:

### Fluent configuration

New instances of `RemoteService` can be created using `RemoteService.Configure()`:

``` cs
RemoteService remoteService = RemoteService
    .Configure()
    .WithMarshaller(simpleMarshaller)
    .ForNetwork(httpRemoteNetworkBuilder)
    .DisableCallDispatching()
    .EnableCallHandling()
        .UsingCommandInvoker(commandInvoker)
        .Asynchronously()
    .Create();
```
