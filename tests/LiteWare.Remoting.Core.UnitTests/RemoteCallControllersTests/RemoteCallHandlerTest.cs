using System;
using System.Threading;
using System.Threading.Tasks;
using LiteWare.Remoting.Core.RemoteCallControllers;
using Moq;
using NUnit.Framework;

namespace LiteWare.Remoting.Core.UnitTests.RemoteCallControllersTests;

[TestFixture]
public class RemoteCallHandlerTest
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Mock<IRemoteServiceMediator> _remoteServiceMediatorMock;
    private Mock<ICommandInvoker> _commandInvokerMock;
    private RemoteCallHandler _remoteCallHandler;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [SetUp]
    public void Setup()
    {
        _remoteServiceMediatorMock = new Mock<IRemoteServiceMediator>();
        _remoteServiceMediatorMock.Setup(m => m.PackAndSend(It.IsAny<RemoteResponse>()));

        _commandInvokerMock = new Mock<ICommandInvoker>();
        _commandInvokerMock.Setup(m => m.InvokeCommand(It.IsAny<string>(), It.IsAny<object?[]>()));

        _remoteCallHandler = new RemoteCallHandler(_remoteServiceMediatorMock.Object, _commandInvokerMock.Object) { HandleReceivedCallsAsynchronously = false };
    }

    [Test]
    public void HandleReceivedCall_Should_InvokeCommand_When_RemoteCallIsFireAndForget()
    {
        RemoteCall remoteCall = RemoteCall.CreateOneWayCall("command", Array.Empty<object>());

        _remoteCallHandler.HandleReceivedCall(remoteCall);

        _commandInvokerMock.Verify(m => m.InvokeCommand(It.IsAny<string>(), It.IsAny<object?[]>()));
    }

    [Test]
    public void HandleReceivedCall_Should_SendSuccessfulResponse_When_RemoteCallIsARequest()
    {
        const int expectedResponseValue = 1;
        _commandInvokerMock.Setup(m => m.InvokeCommand(It.IsAny<string>(), It.IsAny<object?[]>())).Returns(expectedResponseValue);

        Guid guid = Guid.NewGuid();
        RemoteResponse expectedResponse = RemoteResponse.CreateSuccess(guid, expectedResponseValue);
        RemoteCall remoteCall = RemoteCall.CreateRequest(guid, "command", Array.Empty<object>());

        _remoteCallHandler.HandleReceivedCall(remoteCall);

        _commandInvokerMock.Verify(m => m.InvokeCommand(It.IsAny<string>(), It.IsAny<object?[]>()));
        _remoteServiceMediatorMock.Verify(m => m.PackAndSend(expectedResponse));
    }

    [Test]
    public void HandleReceivedCall_Should_SendErrorResponse_When_RemoteCallIsARequest_And_InvokeCommandThrowsAnError()
    {
        const string expectedErrorMessage = "Test";
        _commandInvokerMock.Setup(m => m.InvokeCommand(It.IsAny<string>(), It.IsAny<object?[]>())).Throws(new ArgumentException(expectedErrorMessage));

        Guid guid = Guid.NewGuid();
        RemoteResponse expectedResponse = RemoteResponse.CreateError(guid, expectedErrorMessage);
        RemoteCall remoteCall = RemoteCall.CreateRequest(guid, "command", Array.Empty<object>());

        _remoteCallHandler.HandleReceivedCall(remoteCall);

        _commandInvokerMock.Verify(m => m.InvokeCommand(It.IsAny<string>(), It.IsAny<object?[]>()));
        _remoteServiceMediatorMock.Verify(m => m.PackAndSend(expectedResponse));
    }

    [Test]
    public void HandleReceivedCall_Should_AwaitAndReturnTaskResult_When_InvokeCommandReturnsATask()
    {
        const int expectedResponseValue = 1;

        static int TaskAction()
        {
            Thread.Sleep(100);
            return expectedResponseValue;
        }

        _commandInvokerMock.Setup(m => m.InvokeCommand(It.IsAny<string>(), It.IsAny<object?[]>())).Returns(Task.Run(TaskAction));

        Guid guid = Guid.NewGuid();
        RemoteResponse expectedResponse = RemoteResponse.CreateSuccess(guid, expectedResponseValue);
        RemoteCall remoteCall = RemoteCall.CreateRequest(guid, "command", Array.Empty<object>());

        _remoteCallHandler.HandleReceivedCall(remoteCall);

        _commandInvokerMock.Verify(m => m.InvokeCommand(It.IsAny<string>(), It.IsAny<object?[]>()));
        _remoteServiceMediatorMock.Verify(m => m.PackAndSend(expectedResponse));
    }

    [Test]
    public void HandleReceivedCall_Should_BeHandledAsynchronously_When_HandleReceivedCallsAsynchronouslyIsSetToTrue()
    {
        static void TaskAction()
        {
            Thread.Sleep(100);
        }

        _commandInvokerMock.Setup(m => m.InvokeCommand(It.IsAny<string>(), It.IsAny<object?[]>())).Returns(Task.Run(TaskAction));
        _remoteCallHandler.HandleReceivedCallsAsynchronously = true;
        RemoteCall remoteCall = RemoteCall.CreateOneWayCall("command", Array.Empty<object>());

        _remoteCallHandler.HandleReceivedCall(remoteCall);

        _commandInvokerMock.Verify(m => m.InvokeCommand(It.IsAny<string>(), It.IsAny<object?[]>()), Times.Never);
        Thread.Sleep(100);
        _commandInvokerMock.Verify(m => m.InvokeCommand(It.IsAny<string>(), It.IsAny<object?[]>()), Times.Once);
    }
}