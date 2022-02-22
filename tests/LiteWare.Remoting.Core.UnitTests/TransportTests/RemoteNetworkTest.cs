using LiteWare.Remoting.Core.Transport;
using Moq;
using NUnit.Framework;

namespace LiteWare.Remoting.Core.UnitTests.TransportTests;

[TestFixture]
public class RemoteNetworkTest
{
    #region Stubs

    private class StubRemoteNetwork : RemoteNetwork
    {
        public byte[]? LastSentBytes { get; private set; }

        public StubRemoteNetwork(IRemoteServiceMediator remoteServiceMediator, IMessageSerializer messageSerializer)
            : base(remoteServiceMediator, messageSerializer) { }

        protected override void SendBytes(byte[] messageBytes)
        {
            LastSentBytes = messageBytes;
        }

        public void SimulateMessageReceived(byte[] messageBytes) =>
            HandleReceivedMessageBytes(messageBytes);
    }

    #endregion

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Mock<IRemoteServiceMediator> _remoteServiceMediatorMock;
    private Mock<IMessageSerializer> _messageSerializerMock;
    private StubRemoteNetwork _remoteNetwork;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [SetUp]
    public void Setup()
    {
        _remoteServiceMediatorMock = new Mock<IRemoteServiceMediator>();
        _remoteServiceMediatorMock.Setup(m => m.HandleReceivedMessage(It.IsAny<Message>()));

        _messageSerializerMock = new Mock<IMessageSerializer>();
        _messageSerializerMock.Setup(m => m.Serialize(It.IsAny<Message>())).Returns((Message message) => message.ContentBytes);
        _messageSerializerMock.Setup(m => m.Deserialize(It.IsAny<byte[]>()));

        _remoteNetwork = new StubRemoteNetwork(_remoteServiceMediatorMock.Object, _messageSerializerMock.Object);
    }

    [Test]
    public void Send_Should_SerializeMessageAndSendBytes()
    {
        byte[] expectedSentBytes = { 1, 2, 3 };
        Message message = Message.CreateCall(expectedSentBytes);

        _remoteNetwork.Send(message);

        _messageSerializerMock.Verify(m => m.Serialize(message));
        Assert.That(_remoteNetwork.LastSentBytes, Is.EqualTo(expectedSentBytes));
    }

    [Test]
    public void HandleReceivedMessageBytes_Should_DeserializeMessageAndNotifyReceivedMessage()
    {
        byte[] messageBytes = { 1, 2, 3 };

        _remoteNetwork.SimulateMessageReceived(messageBytes);

        _messageSerializerMock.Verify(m => m.Deserialize(messageBytes));
        _remoteServiceMediatorMock.Verify(m => m.HandleReceivedMessage(It.IsAny<Message>()));
    }
}