using System;
using System.Threading.Tasks;

namespace ViewPort.Core
{
    /// <summary>
    /// An abstraction for UI threads/dispatchers for invoking actions.
    /// </summary>
    public interface IUIThread
    {
        /// <summary>
        /// Invokes a synchronous action on the UI thread.
        /// </summary>
        /// <param name="action">The action.</param>
        void Run(Action action);

        /// <summary>
        /// Invokes an asynchronous action on the UI thread.
        /// </summary>
        /// <param name="action">The action.</param>
        Task RunAsync(Func<Task> action);
    }
}
