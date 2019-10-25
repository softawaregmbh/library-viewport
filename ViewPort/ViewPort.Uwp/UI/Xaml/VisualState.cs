using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace ViewPort.Uwp.UI.Xaml
{
    public class VisualState : DependencyObject
    {
        public static readonly DependencyProperty ShowProperty =
            DependencyProperty.RegisterAttached("Show", typeof(string), typeof(VisualState), new PropertyMetadata(null, (d, e) => OnPropertyChanged(d, e, true)));

        public static readonly DependencyProperty HideProperty =
            DependencyProperty.RegisterAttached("Hide", typeof(string), typeof(VisualState), new PropertyMetadata(null, (d, e) => OnPropertyChanged(d, e, false)));

        public static string GetShow(Windows.UI.Xaml.VisualState state)
        {
            return (string)state.GetValue(ShowProperty);
        }

        public static void SetShow(Windows.UI.Xaml.VisualState state, string value)
        {
            state.SetValue(ShowProperty, value);
        }

        public static string GetHide(Windows.UI.Xaml.VisualState state)
        {
            return (string)state.GetValue(HideProperty);
        }

        public static void SetHide(Windows.UI.Xaml.VisualState state, string value)
        {
            state.SetValue(HideProperty, value);
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e, bool show)
        {
            if (e.NewValue != null)
            {
                var elements = ((string)e.NewValue).Split(' ', ',');

                var state = (Windows.UI.Xaml.VisualState)d;
                
                if (state.Storyboard == null)
                {
                    state.Storyboard = new Storyboard();
                }

                foreach (var element in elements)
                {
                    string[] nameAndDuration = element.Split('@');
                    string name = nameAndDuration[0];
                    TimeSpan duration = nameAndDuration.Length == 1 ? TimeSpan.Zero : TimeSpan.Parse(nameAndDuration[1]);

                    var visibilityAnimation = new ObjectAnimationUsingKeyFrames();
                    Storyboard.SetTargetName(visibilityAnimation, name);
                    Storyboard.SetTargetProperty(visibilityAnimation, "Visibility");
                    visibilityAnimation.KeyFrames.Add(new DiscreteObjectKeyFrame() { KeyTime = KeyTime.FromTimeSpan(show ? TimeSpan.Zero : duration), Value = show ? Visibility.Visible : Visibility.Collapsed });

                    state.Storyboard.Children.Add(visibilityAnimation);

                    if (duration.TotalMilliseconds > 0)
                    {
                        var opacityAnimation = new DoubleAnimation();
                        Storyboard.SetTargetName(opacityAnimation, name);
                        Storyboard.SetTargetProperty(opacityAnimation, "Opacity");
                        opacityAnimation.From = show ? 0.0 : 1.0;
                        opacityAnimation.To = show ? 1.0 : 0.0;
                        opacityAnimation.Duration = duration;

                        state.Storyboard.Children.Add(opacityAnimation);
                    }
                }
            }
        }
    }
}
