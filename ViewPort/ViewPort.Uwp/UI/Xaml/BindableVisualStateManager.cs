using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ViewPort.Uwp.UI.Xaml
{
    public class BindableVisualStateManager : VisualStateManager
    {
        public static readonly DependencyProperty VisualStateProperty =
            DependencyProperty.RegisterAttached("VisualState", typeof(object), typeof(BindableVisualStateManager), new PropertyMetadata(null, OnVisualStateChanged));

        public static object GetVisualState(DependencyObject obj)
        {
            return (object)obj.GetValue(VisualStateProperty);
        }

        public static void SetVisualState(DependencyObject obj, object value)
        {
            obj.SetValue(VisualStateProperty, value);
        }

        public bool GoToState(FrameworkElement element, string stateName, bool useTransitions)
        {
            foreach (var group in VisualStateManager.GetVisualStateGroups(element))
            {
                foreach (var state in group.States) 
                {
                    if (state.Name == stateName)
                    {
                        return this.GoToStateCore(null, element, stateName, group, state, useTransitions);
                    }
                }
            }

            return false;
        }

        protected override bool GoToStateCore(Control control, FrameworkElement templateRoot, string stateName, VisualStateGroup group, Windows.UI.Xaml.VisualState state, bool useTransitions)
        {
            return base.GoToStateCore(control ?? new ContentControl(), templateRoot, stateName, group, state, useTransitions);
        }

        private static void OnVisualStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                return;
            }

            var element = d as FrameworkElement;
            if (element != null)
            {
                var manager = (BindableVisualStateManager)VisualStateManager.GetCustomVisualStateManager(element);
                if (manager != null)
                {
                    manager.GoToState(element, e.NewValue.ToString(), true);
                }
            }
        }
    }
}
