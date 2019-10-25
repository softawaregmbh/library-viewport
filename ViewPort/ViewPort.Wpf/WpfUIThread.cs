using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using ViewPort.Core;

namespace ViewPort.Wpf
{
    /// <summary>
    /// Implementation of the <see cref="IUIThread"/> interface using a WPF dispatcher.
    /// </summary>
    public class WpfUIThread : IUIThread
    {
        private readonly Dispatcher dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfUIThread"/> class using the <see cref="Dispatcher.CurrentDispatcher"/> property.
        /// </summary>
        public WpfUIThread()
            : this(Dispatcher.CurrentDispatcher)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfUIThread"/> class using the provided dispatcher.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        public WpfUIThread(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        /// <inheritdoc />
        public void Run(Action action)
        {
            this.dispatcher.Invoke(action);
        }

        /// <inheritdoc />
        public Task RunAsync(Func<Task> action)
        {
            var taskSource = new TaskCompletionSource<bool>();

            this.dispatcher.BeginInvoke(new Action(async () =>
            {
                try
                {
                    await action();
                    taskSource.SetResult(true);
                }
                catch (Exception e)
                {
                    taskSource.SetException(e);
                }
            }));

            return taskSource.Task;
        }
    }
}
