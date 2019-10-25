using System;

namespace ViewPort.Messaging
{
    internal class Registration
    {
        private Registration(WeakReference receiver, Type messageType)
        {
            this.ReceiverReference = receiver;
            this.MessageType = messageType;
        }

        public WeakReference ReceiverReference { get; private set; }
        public Type MessageType { get; private set; }

        public static Registration For<TMessage>(IReceiver<TMessage> receiver)
        {
            return new Registration(new WeakReference(receiver), typeof(TMessage));
        }
    }
}
