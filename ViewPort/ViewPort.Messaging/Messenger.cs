using System;
using System.Collections.Generic;

namespace ViewPort.Messaging
{
    public class Messenger : IMessenger
    {
        private LinkedList<Registration> receivers;

        public Messenger()
        {
            this.receivers = new LinkedList<Registration>();
        }

        public IDisposable Register<TMessage>(IReceiver<TMessage> receiver)
        {
            var reference = new ReceiverReference<TMessage>(receiver, this);
            this.receivers.AddLast(Registration.For(receiver));
            return reference;
        }

        public IDisposable Register<TMessage>(Action<TMessage> receive)
        {
            return this.Register(new DelegateReceiver<TMessage>(receive));
        }

        public void Deregister<TMessage>(IReceiver<TMessage> receiver)
        {
            var node = this.receivers.First;
            while (node != null)
            {
                var next = node.Next;
                if (!node.Value.ReceiverReference.IsAlive || node.Value.ReceiverReference.Target == receiver)
                {
                    this.receivers.Remove(node);
                }

                node = next;
            }
        }

        public void Send<TMessage>(TMessage message)
        {
            var node = this.receivers.First;
            while (node != null)
            {
                var next = node.Next;
                var receiver = node.Value.ReceiverReference.Target as IReceiver<TMessage>;
                if (receiver != null && node.Value.MessageType == typeof(TMessage))
                {
                    receiver.Receive(message);
                }
                else if (!node.Value.ReceiverReference.IsAlive)
                {
                    this.receivers.Remove(node);
                }

                node = next;
            }
        }
    }
}
