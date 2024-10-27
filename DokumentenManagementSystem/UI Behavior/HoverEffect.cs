using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DokumentenManagementSystem.UI_Behavior
{
    public class HoverEffect
    {
        private const double ScaleFactor = 1.1;
        private const double AnimationDurationInSeconds = 0.2;

        public static void ResizeOnHover(object sender, MouseEventArgs e)
        {
            if (sender is FrameworkElement element)
                ApplyHoverEffect(element, e.RoutedEvent == UIElement.MouseEnterEvent);
        }

        private static void ApplyHoverEffect(FrameworkElement element, bool isMouseEnter)
        {
            var scaleTransform = element.RenderTransform as ScaleTransform ?? new ScaleTransform(1, 1);
            element.RenderTransform = scaleTransform;
            element.RenderTransformOrigin = new Point(0.5, 0.5);

            double targetScale = isMouseEnter ? ScaleFactor : 1;

            var scaleXAnimation = new DoubleAnimation(targetScale, TimeSpan.FromSeconds(AnimationDurationInSeconds))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };

            var scaleYAnimation = new DoubleAnimation(targetScale, TimeSpan.FromSeconds(AnimationDurationInSeconds))
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleXAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleYAnimation);
        }
    }
}