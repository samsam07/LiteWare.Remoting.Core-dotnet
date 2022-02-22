using System;
using LiteWare.Remoting.Core.Transport;
using NUnit.Framework;

namespace LiteWare.Remoting.Core.UnitTests.TransportTests;

[TestFixture]
public class MessageSerializerTest
{
    [Test]
    public void Serialize_Should_OnlySerializeIntent_When_NoMessageContentIsPresent()
    {
        byte[] expectedBytes = { (byte)MessageIntent.Call };
        Message message = new(MessageIntent.Call, Array.Empty<byte>());
        MessageSerializer serializer = new();

        byte[] bytes = serializer.Serialize(message);

        Assert.That(bytes, Is.EqualTo(expectedBytes));
    }

    [Test]
    public void Serialize_Should_SerializeIntentWithMessageContent_When_MessageContentIsPresent()
    {
        byte[] expectedBytes = { (byte)MessageIntent.Call, 1, 2, 3 };
        Message message = new(MessageIntent.Call, new byte[] { 1, 2, 3 });
        MessageSerializer serializer = new();

        byte[] bytes = serializer.Serialize(message);

        Assert.That(bytes, Is.EqualTo(expectedBytes));
    }

    [Test]
    public void Deserialize_Should_ThrowArgumentException_When_EmptyMessageBytesAreProvided()
    {
        MessageSerializer serializer = new();

        Assert.Throws<ArgumentException>(() => serializer.Deserialize(Array.Empty<byte>()));
    }

    [Test]
    public void Deserialize_Should_DeserializeMessageWithOnlyIntent_When_NoContentIsPresentInMessageBytes()
    {
        byte[] messageBytes = { (byte)MessageIntent.Call };
        MessageSerializer serializer = new();

        Message message = serializer.Deserialize(messageBytes);

        Assert.That(message.Intent, Is.EqualTo(MessageIntent.Call));
        Assert.That(message.ContentBytes, Is.Empty);
    }

    [Test]
    public void Deserialize_Should_DeserializeMessageWithIntentAndContent_When_ContentIsPresentInMessageBytes()
    {
        byte[] messageBytes = { (byte)MessageIntent.Call, 1, 2, 3 };
        MessageSerializer serializer = new();

        Message message = serializer.Deserialize(messageBytes);

        Assert.That(message.Intent, Is.EqualTo(MessageIntent.Call));
        Assert.That(message.ContentBytes, Is.EqualTo(new byte[] { 1, 2, 3 }));
    }
}