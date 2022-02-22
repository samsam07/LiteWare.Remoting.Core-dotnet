using System;
using System.Threading;
using System.Threading.Tasks;
using LiteWare.Remoting.Core.RemoteCallControllers;
using NUnit.Framework;

namespace LiteWare.Remoting.Core.UnitTests.RemoteCallControllersTests;

[TestFixture]
public class RemoteResponseAwaiterTest
{
    private static void DelayAndSignalResponse(RemoteResponseAwaiter responseAwaiter, RemoteResponse response, int delay = 100)
    {
        Task.Run
        (
            () =>
            {
                Thread.Sleep(delay);
                responseAwaiter.SignalResponse(response);
            }
        );
    }

    [Test]
    public async Task WaitForResponse_Should_ReturnNull_When_TimeoutExpires()
    {
        TimeSpan timeout = TimeSpan.FromMilliseconds(1);
        using RemoteResponseAwaiter responseAwaiter = new(timeout);

        RemoteResponse? response = await responseAwaiter.WaitForResponse();

        Assert.That(response, Is.Null);
    }

    [Test]
    public async Task WaitForResponse_Should_Response_When_ResponseIsSignaledWithinTimeout()
    {
        RemoteResponse expectedResponse = RemoteResponse.CreateSuccess(Guid.NewGuid(), "Test");
        TimeSpan timeout = TimeSpan.FromSeconds(1);
        using RemoteResponseAwaiter responseAwaiter = new(timeout);

        DelayAndSignalResponse(responseAwaiter, expectedResponse);
        RemoteResponse? response = await responseAwaiter.WaitForResponse();

        Assert.That(response, Is.EqualTo(expectedResponse));
    }
}