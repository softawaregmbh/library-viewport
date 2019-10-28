using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace softaware.ViewPort.Commands
{
    /// <summary>
    /// Implementation of <see cref="ICommand"/> without parameters and an asynchronously executed action.
    /// </summary>
    public class AsyncCommand : ICommand
    {
        private readonly Func<Task> executeAsync;
        private readonly Func<bool> canExecute;

        /// <inheritdoc />
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncCommand"/> class.
        /// </summary>
        /// <param name="executeAsync">The implementation of the command's <see cref="ExecuteAsync"/> method.</param>
        /// <param name="canExecute">The implementation of the command's <see cref="CanExecute"/> method.</param>
        public AsyncCommand(Func<Task> executeAsync, Func<bool> canExecute = null)
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
        /// <returns>
        ///   <c>true</c> if this command can be executed; otherwise, <c>false</c>.
        /// </returns>
        public bool CanExecute() => this.canExecute?.Invoke() ?? true;

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        public async Task ExecuteAsync() => await this.executeAsync();

        /// <inheritdoc />
        bool ICommand.CanExecute(object parameter) => this.CanExecute();

        /// <inheritdoc />
        async void ICommand.Execute(object parameter) => await this.ExecuteAsync();
    }
}
