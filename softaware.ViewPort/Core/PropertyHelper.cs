using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace softaware.ViewPort
{
    /// <summary>
    /// Helper methods used to identify properties by lambda expressions.
    /// </summary>
    public static class PropertyHelper
    {
        /// <summary>
        /// Gets the name of a property identified by a lambda expression. Use this overload if you want to specify the property from outside the defining class.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="object">The object.</param>
        /// <param name="propertyExpression">The lambda expression identifying the property.</param>
        /// <returns>The name of the property.</returns>
        public static string GetPropertyName<T, TProperty>(this T @object, Expression<Func<T, TProperty>> propertyExpression)
        {
            return GetPropertyName(propertyExpression);
        }

        /// <summary>
        /// Gets the name of a property identified by a lambda expression. Use this overload if you want to specify the property from inside the defining class.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns>The name of the property.</returns>
        public static string GetPropertyName<T, TProperty>(this T @object, Expression<Func<TProperty>> propertyExpression)
        {
            return GetPropertyName(propertyExpression);
        }  

        /// <summary>
        /// Creates and registers a PropertyChangedEventHandler that executes the given action if a property changes.
        /// The event handler is returned and can be deregistered directly via -=.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="notifyPropertyChanged">The object which raises the PropertyChanged event.</param>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="action">The action that should be executed. The parameter is the new value of the property that changes.</param>
        /// <returns>The PropertyChangedEventHandler.</returns>
        public static PropertyChangedEventHandler ExecuteOnPropertyChanged<T, TProperty>(this T notifyPropertyChanged, Expression<Func<T, TProperty>> propertyExpression, Action<TProperty> action)
            where T : INotifyPropertyChanged
        {
            var propertyName = GetPropertyName(propertyExpression);
            var compiledExpression = propertyExpression.Compile();
            
            void OnPropertyChanged(object s, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == propertyName)
                {
                    action(compiledExpression(notifyPropertyChanged));
                }
            }

            notifyPropertyChanged.PropertyChanged += OnPropertyChanged;
            return OnPropertyChanged;
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <param name="propertyExpression">The property expression. This can either be an Expression&lt;Func&lt;T, TProperty&gt;&gt; or an Expression&lt;TProperty&gt;.</param>
        /// <returns>The name of the property.</returns>
        private static string GetPropertyName(LambdaExpression propertyExpression)
        {
            MemberExpression memberExpression;

            if (propertyExpression.Body is UnaryExpression unaryExpression)
            {
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = propertyExpression.Body as MemberExpression;
            }

            if (memberExpression == null)
            {
                throw new ArgumentOutOfRangeException(nameof(propertyExpression), "Invalid property expression type: " + propertyExpression.Body.GetType());
            }

            return memberExpression.Member.Name;
        }
    }
}
