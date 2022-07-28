using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Nighthold_Login.Nighthold
{
    class AnimHandler
    {
        public static void ScaleIn(FrameworkElement element)
        {
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1, 1);

            element.RenderTransformOrigin = new Point(0.5, 0.5);
            element.RenderTransform = scale;

            DoubleAnimation scaleAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(250),
                From = 0
            };

            // Animate opacity
            Storyboard.SetTarget(scaleAnimation, element);
            Storyboard.SetTargetProperty(scaleAnimation, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(scaleAnimation);
            storyboard.Begin();

            // Animate scale X
            Storyboard.SetTarget(scaleAnimation, element);
            Storyboard.SetTargetProperty(scaleAnimation, new PropertyPath("RenderTransform.ScaleX"));
            storyboard.Children.Add(scaleAnimation);
            storyboard.Begin();

            // Animate scale Y
            Storyboard.SetTarget(scaleAnimation, element);
            Storyboard.SetTargetProperty(scaleAnimation, new PropertyPath("RenderTransform.ScaleY"));
            storyboard.Children.Add(scaleAnimation);
            storyboard.Begin();
        }

        public static void MoveUpAndFadeIn(FrameworkElement element)
        {
            // Animate opacity
            Storyboard storyboard = new Storyboard();
            DoubleAnimation FadeInAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(1000),
                From = 0,
            };
            Storyboard.SetTarget(FadeInAnimation, element);
            Storyboard.SetTargetProperty(FadeInAnimation, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(FadeInAnimation);
            storyboard.Begin();

            TranslateTransform trans = new TranslateTransform();
            element.RenderTransform = trans;
            DoubleAnimation moveInUpAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(600),
                From = 10,
            };

            trans.BeginAnimation(TranslateTransform.YProperty, moveInUpAnimation);
        }

        public static Storyboard SpinForever(FrameworkElement element)
        {
            Storyboard storyboard = new Storyboard();

            RotateTransform rotate = new RotateTransform(0);

            element.RenderTransformOrigin = new Point(0.5, 0.5);
            element.RenderTransform = rotate;

            DoubleAnimation spinAnimation = new DoubleAnimation()
            {
                BeginTime = TimeSpan.FromSeconds(0),
                Duration = TimeSpan.FromMilliseconds(2000),
                RepeatBehavior = RepeatBehavior.Forever,
                From = 0,
                To = 360, 
            };

            // Animate rotation
            Storyboard.SetTarget(spinAnimation, element);
            Storyboard.SetTargetProperty(spinAnimation, new PropertyPath("RenderTransform.(RotateTransform.Angle)"));
            storyboard.Children.Add(spinAnimation);
            storyboard.Begin();

            return storyboard;
        }
    }
}
