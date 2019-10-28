namespace softaware.ViewPort.Messaging
{
    public class Message<T>
    {
        public Message(T content)
        {
            this.Content = content;
        }

        public T Content { get; private set; }
    }
}
