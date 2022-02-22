using System;
using System.Threading;
using System.Threading.Tasks;
using LiteWare.Remoting.Core.Exceptions;
using LiteWare.Remoting.Core.RemoteCallControllers;
using Moq;
using NUnit.Framework;

namespace LiteWare.Remoting.Core.UnitTests.RemoteCallControllersTests;

[TestFixture]
public class RemoteCallDispatcherTest
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Mock<IRemoteServiceMediator> _remoteServiceMediatorMock;
    private RemoteCallDispatcher _remoteCallDispatcher;
    private RemoteCall _sentCall;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private void DelayAndSendResponse(object? responseValue = null, string? errorMessage = null, int delay = 100)
    {
        Task.Run
        (
            () =>
            {
                Thread.Sleep(delay);

                ResponseStatus responseStatus = (responseValue is null)? ResponseStatus.Error : ResponseStatus.Success;
                RemoteResponse response = new(responseStatus, _sentCall.Reference, responseValue, errorMessage);
                _remoteCallDispatcher.HandleReceivedResponse(response);
            }
        );
    }

    [SetUp]
    public void Setup()
    {
        _remoteServiceMediatorMock = new Mock<IRemoteServiceMediator>();
        _remoteServiceMediatorMock.Setup(m => m.PackAndSend(It.IsAny<RemoteCall>())).Callback((RemoteCall remoteCall) => { _sentCall = remoteCall; });

        _remoteCallDispatcher = new RemoteCallDispatcher(_remoteServiceMediatorMock.Object) { RequestTimeout = TimeSpan.FromSeconds(1) };
    }

    [Test]
    public void Call_Should_PackAndSendFireAndForgetRemoteCall()
    {
        const string command = "command";
        object?[] parameters = Array.Empty<object?>();
        RemoteCall expectedRemoteCall = RemoteCall.CreateOneWayCall(command, parameters);

        _remoteCallDispatcher.Call(command, parameters);

        _remoteServiceMediatorMock.Verify(m => m.PackAndSend(expectedRemoteCall));
    }

    [Test]
    public async Task RequestAsync_Should_ReturnResponseValue_When_ResponseWasReceived_And_ResponseIsSuccess()
    {
        const int expectedResponseValue = 10;
        DelayAndSendResponse(responseValue: expectedResponseValue);

        object? responseValue = await _remoteCallDispatcher.RequestAsync("command", Array.Empty<object?>());

        _remoteServiceMediatorMock.Verify(m => m.PackAndSend(It.IsAny<RemoteCall>()));
        Assert.That(responseValue, Is.EqualTo(expectedResponseValue));
    }

    [Test]
    public void RequestAsync_Should_ThrowRequestInvokeException_When_ResponseWasReceived_And_ResponseIsError()
    {
        const string command = "command";
        object?[] parameters = { 1, 2, 3 };
        const string errorMessage = "Something occurred.";
        DelayAndSendResponse(errorMessage: errorMessage);

        RequestInvokeException exception = Assert.ThrowsAsync<RequestInvokeException>(async () => await _remoteCallDispatcher.RequestAsync(command, parameters))!;

        Assert.That(exception.Message, Is.EqualTo(errorMessage));
        Assert.That(exception.RemoteCall, Is.Not.Null);
        Assert.That(exception.RemoteCall!.CallType, Is.EqualTo(CallType.AwaitCallback));
        Assert.That(exception.RemoteCall!.Command, Is.EqualTo(command));
        Assert.That(exception.RemoteCall!.Parameters, Is.EqualTo(parameters));
        _remoteServiceMediatorMock.Verify(m => m.PackAndSend(It.IsAny<RemoteCall>()));
    }

    [Test]
    public void RequestAsync_Should_ThrowRequestTimeoutException_When_ResponseWasNotReceivedWithinTimeout()
    {
        const string command = "command";
        object?[] parameters = { 1, 2, 3 };

        RequestTimeoutException exception = Assert.ThrowsAsync<RequestTimeoutException>(async () => await _remoteCallDispatcher.RequestAsync(command, parameters))!;

        Assert.That(exception.RemoteCall, Is.Not.Null);
        Assert.That(exception.RemoteCall!.CallType, Is.EqualTo(CallType.AwaitCallback));
        Assert.That(exception.RemoteCall!.Command, Is.EqualTo(command));
        Assert.That(exception.RemoteCall!.Parameters, Is.EqualTo(parameters));
    }
}