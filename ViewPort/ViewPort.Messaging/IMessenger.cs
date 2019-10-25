using System;

namespace ViewPort.Messaging
{
    public interface IMessenger
    {
        void Send<TMessage>(TMessage message);
        IDisposable Register<TMessage>(IReceiver<TMessage> receiver);
        IDisposable Register<TMessage>(Action<TMessage> receive);
        void Deregister<TMessage>(IReceiver<TMessage> receiver);
    }
}
