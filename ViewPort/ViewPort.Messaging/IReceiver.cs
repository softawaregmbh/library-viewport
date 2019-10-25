namespace ViewPort.Messaging
{
    public interface IReceiver<TMessage>
    {
        void Receive(TMessage message);
    }
}
