using System;
using System.Threading.Tasks;
using softaware.ViewPort.Core;
using Windows.UI.Core;

namespace softaware.ViewPort.WinRT
{
    public class UwpUIThread : IUIThread
    {       
        private readonly CoreDispatcher dispatcher;

        public UwpUIThread()
            : this(CoreWindow.GetForCurrentThread().Dispatcher)
        {
        }

        public UwpUIThread(CoreDispatcher dispatcher)
        {
            this.dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        public void Run(Action action)
        {
            if (this.dispatcher.HasThreadAccess)
            {
                action();
            }
            else
            {
                this.dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(action)).AsTask().Wait();
            }
        }

        public Task RunAsync(Func<Task> action)
        {
            var taskSource = new TaskCompletionSource<bool>();
            
            var result = this.dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                new DispatchedHandler(async () =>
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
