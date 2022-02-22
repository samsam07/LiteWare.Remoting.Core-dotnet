using System;
using LiteWare.Remoting.Core.Transport;
using Moq;
using NUnit.Framework;

namespace LiteWare.Remoting.Core.UnitTests;

[TestFixture]
public class MessagePackerTest
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Mock<IMarshaller> _marshallerMock;
    private MessagePacker _messagePacker;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [SetUp]
    public void Setup()
    {
        _marshallerMock = new Mock<IMarshaller>();
        _messagePacker = new MessagePacker(_marshallerMock.Object);
    }

    [Test]
    public void PackRemoteCall_Should_ReturnCallIntentMessage()
    {
        RemoteCall remoteCall = RemoteCall.CreateOneWayCall("command", Array.Empty<object>());
        byte[] messageBytes = { 1, 2, 3 };
        _marshallerMock.Setup(m => m.MarshallRemoteCall(remoteCall)).Returns(messageBytes);

        Message message = _messagePacker.PackRemoteCall(remoteCall);

        Assert.That(message.Intent, Is.EqualTo(MessageIntent.Call));
        Assert.That(message.ContentBytes, Is.EqualTo(messageBytes));
    }

    [TestCase(MessageIntent.Response, null, TestName = "UnpackRemoteCall_Should_ThrowArgumentException_When_MessageIntentIsNotCall", ExpectedResult = "Wrong intent for remote call unpack.")]
    [TestCase(MessageIntent.Call, null, TestName = "UnpackRemoteCall_Should_ThrowArgumentException_When_MessageContentIsNull", ExpectedResult = "Content bytes cannot be null or empty.")]
    [TestCase(MessageIntent.Call, new byte[] { }, TestName = "UnpackRemoteCall_Should_ThrowArgumentException_When_MessageContentIsEmpty", ExpectedResult = "Content bytes cannot be null or empty.")]
    public string UnpackRemoteCall_Should_ThrowArgumentException(MessageIntent messageIntent, byte[] messageContent)
    {
        Message message = new(messageIntent, messageContent);

        ArgumentException exception = Assert.Throws<ArgumentException>(() => _messagePacker.UnpackRemoteCall(message))!;
        return exception.Message;
    }

    [Test]
    public void PackRemoteResponse_Should_ReturnResponseIntentMessage()
    {
        RemoteResponse remoteResponse = RemoteResponse.CreateSuccess(Guid.NewGuid(), 1);
        byte[] messageBytes = { 1, 2, 3 };
        _marshallerMock.Setup(m => m.MarshallRemoteResponse(remoteResponse)).Returns(messageBytes);

        Message message = _messagePacker.PackRemoteResponse(remoteResponse);

        Assert.That(message.Intent, Is.EqualTo(MessageIntent.Response));
        Assert.That(message.ContentBytes, Is.EqualTo(messageBytes));
    }

    [TestCase(MessageIntent.Call, null, TestName = "UnpackRemoteResponse_Should_ThrowArgumentException_When_MessageIntentIsNotCall", ExpectedResult = "Wrong intent for remote response unpack.")]
    [TestCase(MessageIntent.Response, null, TestName = "UnpackRemoteResponse_Should_ThrowArgumentException_When_MessageContentIsNull", ExpectedResult = "Content bytes cannot be null or empty.")]
    [TestCase(MessageIntent.Response, new byte[] { }, TestName = "UnpackRemoteResponse_Should_ThrowArgumentException_When_MessageContentIsEmpty", ExpectedResult = "Content bytes cannot be null or empty.")]
    public string UnpackRemoteResponse_Should_ThrowArgumentException(MessageIntent messageIntent, byte[] messageContent)
    {
        Message message = new(messageIntent, messageContent);

        ArgumentException exception = Assert.Throws<ArgumentException>(() => _messagePacker.UnpackRemoteResponse(message))!;
        return exception.Message;
    }
}