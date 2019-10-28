using System;
using System.Windows.Input;

namespace softaware.ViewPort.Commands
{
    /// <summary>
    /// Implementation of <see cref="ICommand"/> without parameters and a synchronously executed action.
    /// </summary>
    public class Command : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        /// <inheritdoc />
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="execute">The implementation of the command's <see cref="Execute"/> method.</param>
        /// <param name="canExecute">The implementation of the command's <see cref="CanExecute"/> method.</param>
        public Command(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        /// <inheritdoc />
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
        public void Execute() => this.execute();

        /// <inheritdoc />
        bool ICommand.CanExecute(object parameter) => this.CanExecute();

        /// <inheritdoc />
        void ICommand.Execute(object parameter) => this.Execute();
    }
}
