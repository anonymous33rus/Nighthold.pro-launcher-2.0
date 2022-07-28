using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Nighthold_Launcher.Nighthold
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

        public static void FadeIn(FrameworkElement element, int millisecondsDuration)
        {
            element.Visibility = Visibility.Visible;

            // Animate opacity
            Storyboard storyboard = new Storyboard();
            DoubleAnimation FadeInAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(millisecondsDuration),
                From = 0,
            };
            Storyboard.SetTarget(FadeInAnimation, element);
            Storyboard.SetTargetProperty(FadeInAnimation, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(FadeInAnimation);
            storyboard.Begin();
        }

        public static async void FadeOut(FrameworkElement element, int millisecondsDuration)
        {
            // Animate opacity
            Storyboard storyboard = new Storyboard();
            DoubleAnimation FadeInAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(millisecondsDuration),
                To = 0,
            };
            Storyboard.SetTarget(FadeInAnimation, element);
            Storyboard.SetTargetProperty(FadeInAnimation, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(FadeInAnimation);
            storyboard.Begin();

            await Task.Delay(millisecondsDuration);
            element.Visibility = Visibility.Hidden;
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

            // Animate position
            TranslateTransform trans = new TranslateTransform();
            element.RenderTransform = trans;
            DoubleAnimation moveInUpAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(600),
                From = 10,
            };

            trans.BeginAnimation(TranslateTransform.YProperty, moveInUpAnimation);
        }

        public static async Task MoveUpAndFadeInThenFadeOut(FrameworkElement element, int fadeOutMillisecondsDelay)
        {
            // Animate opacity to fade in
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

            // Animate position
            TranslateTransform trans = new TranslateTransform();
            element.RenderTransform = trans;
            DoubleAnimation moveInUpAnimation = new DoubleAnimation()
            {
                //BeginTime = TimeSpan.FromMilliseconds(2000),
                Duration = TimeSpan.FromMilliseconds(600),
                From = 10,
            };

            trans.BeginAnimation(TranslateTransform.YProperty, moveInUpAnimation);

            await Task.Delay(fadeOutMillisecondsDelay);

            //Animate opacity to fade out
            Storyboard storyboard2 = new Storyboard();
            DoubleAnimation FadeOutAnimation = new DoubleAnimation()
            {
                //BeginTime = TimeSpan.FromMilliseconds(fadeOutMillisecondsDelay),
                Duration = TimeSpan.FromMilliseconds(1000),
                To = 0,
            };
            Storyboard.SetTarget(FadeOutAnimation, element);
            Storyboard.SetTargetProperty(FadeOutAnimation, new PropertyPath(UIElement.OpacityProperty));
            storyboard2.Children.Add(FadeOutAnimation);
            storyboard2.Begin();
        }

        public static void MoveUpAndFadeIn300Ms(FrameworkElement element)
        {
            // Animate opacity
            Storyboard storyboard = new Storyboard();
            DoubleAnimation FadeInAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(300),
                From = 0,
            };
            Storyboard.SetTarget(FadeInAnimation, element);
            Storyboard.SetTargetProperty(FadeInAnimation, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(FadeInAnimation);
            storyboard.Begin();

            // Animate position
            TranslateTransform trans = new TranslateTransform();
            element.RenderTransform = trans;
            DoubleAnimation moveInUpAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(300),
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

        public static void ToSpecificHeight(FrameworkElement element, double height)
        {
            // Animate height
            Storyboard storyboard = new Storyboard();
            DoubleAnimation HeightAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(300),
                To = height,
            };
            Storyboard.SetTarget(HeightAnimation, element);
            Storyboard.SetTargetProperty(HeightAnimation, new PropertyPath(FrameworkElement.HeightProperty));
            storyboard.Children.Add(HeightAnimation);
            storyboard.Begin();
        }
    }
}
