using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace softaware.ViewPort.Commands
{
    /// <summary>
    /// Implementation of <see cref="ICommand" /> with a typed parameter and an asynchronously executed action.
    /// </summary>
    /// <typeparam name="T">The parameter type.</typeparam>
    public class AsyncCommand<T> : ICommand
    {
        private readonly Func<T, Task> executeAsync;
        private readonly Func<T, bool> canExecute;

        /// <inheritdoc />
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncCommand{T}"/> class.
        /// </summary>
        /// <param name="executeAsync">The implementation of the command's <see cref="ExecuteAsync"/> method.</param>
        /// <param name="canExecute">The implementation of the command's <see cref="CanExecute"/> method.</param>
        public AsyncCommand(Func<T, Task> executeAsync, Func<T, bool> canExecute = null)
        {
            this.executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event.
        /// </summary>
        public void RaiseCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        /// <returns>
        ///   <c>true</c> if this command can be executed; otherwise, <c>false</c>.
        /// </returns>
        public bool CanExecute(T parameter) => this.canExecute?.Invoke(parameter) ?? true;

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>        
        public async Task ExecuteAsync(T parameter) => await this.executeAsync(parameter);

        /// <inheritdoc />
        bool ICommand.CanExecute(object parameter) => this.CanExecute((T)parameter);

        /// <inheritdoc />
        async void ICommand.Execute(object parameter) => await this.ExecuteAsync((T)parameter);
    }
}
