using System;

namespace softaware.ViewPort.Messaging
{
    public class DelegateReceiver<TMessage> : IReceiver<TMessage>
    {
        private readonly Action<TMessage> receive;

        public DelegateReceiver(Action<TMessage> receive)
        {
            this.receive = receive ?? throw new ArgumentNullException("receive");
        }

        public void Receive(TMessage message)
        {
            this.receive(message);
        }
    }
}
