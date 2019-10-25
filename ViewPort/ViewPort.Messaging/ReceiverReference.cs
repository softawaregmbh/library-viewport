using System;

namespace ViewPort.Messaging
{
    internal class ReceiverReference<T> : IDisposable
    {
        private readonly IMessenger messenger;
        private IReceiver<T> receiver;

        public ReceiverReference(IReceiver<T> receiver, IMessenger messenger)
        {
            this.receiver = receiver;
            this.messenger = messenger;
        }

        public void Dispose()
        {
            this.messenger.Deregister(this.receiver);
            this.receiver = null;
        }
    }
}
