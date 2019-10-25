using System;
using System.Windows.Input;

namespace ViewPort.Commands
{
    /// <summary>
    /// Implementation of <see cref="ICommand" /> with a typed parameter and a synchronously executed action.
    /// </summary>
    /// <typeparam name="T">The parameter type.</typeparam>
    public class Command<T> : ICommand
    {
        private readonly Action<T> execute;
        private readonly Func<T, bool> canExecute;

        /// <inheritdoc />
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command{T}"/> class.
        /// </summary>
        /// <param name="execute">The implementation of the command's <see cref="Execute"/> method.</param>
        /// <param name="canExecute">The implementation of the command's <see cref="CanExecute"/> method.</param>
        public Command(Action<T> execute, Func<T, bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        /// <inheritdoc />
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
        public void Execute(T parameter) => this.execute(parameter);

        /// <inheritdoc />
        bool ICommand.CanExecute(object parameter) => this.CanExecute((T)parameter);

        /// <inheritdoc />
        void ICommand.Execute(object parameter) => this.Execute((T)parameter);
    }
}
