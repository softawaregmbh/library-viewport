using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace softaware.ViewPort.Core
{
    /// <summary>
    /// Intended to use as a base class that provides an implementation of the <see cref="INotifyPropertyChanged"/> interface.
    /// </summary>
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property. Can be omitted if called from the property itself.</param>
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            this.VerifyPropertyName(propertyName);

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertyExpression">A lambda expression identifying the property.</param>
        protected void RaisePropertyChanged<TProperty>(Expression<Func<TProperty>> propertyExpression)
        {
            this.RaisePropertyChanged(this.GetPropertyName(propertyExpression));
        }

        /// <summary>
        /// Sets the backing field of a property to the given value, performing a check if the value changed and raising the <see cref="PropertyChanged"/> event if that's the case.
        /// The type's default equality comparer is used to check if the value changed.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="backingField">The backing field.</param>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">The name of the property. Can be omitted if called from the property itself.</param>
        /// <returns><c>True</c> if the value changed and the event was raised, <c>false</c> otherwise.</returns>
        protected bool SetProperty<TProperty>(ref TProperty backingField, TProperty value, [CallerMemberName]string propertyName = "")
        {
            var comparer = EqualityComparer<TProperty>.Default;
            if (!comparer.Equals(value, backingField))
            {
                backingField = value;
                this.RaisePropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Verifies that a property of the given name exists and returns the name.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The name of the property.</returns>
        /// <remarks>
        /// The check is only performed in DEBUG mode.
        /// </remarks>
        [Conditional("DEBUG")]
        private void VerifyPropertyName(string propertyName)
        {
            var type = this.GetType();
            Debug.Assert(type.GetRuntimeProperty(propertyName) != null, $"Property \"{propertyName}\" does not exist on type \"{type.FullName}\"");
        }
    }
}
