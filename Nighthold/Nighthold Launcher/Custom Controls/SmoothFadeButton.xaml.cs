using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Nighthold_Launcher.Custom_Controls
{
    #region Inverted Bolean Converter for Visibility
    public class BooleanConverter<T> : IValueConverter
    {
        public BooleanConverter(T trueValue, T falseValue)
        {
            True = trueValue;
            False = falseValue;
        }

        public T True { get; set; }
        public T False { get; set; }

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool && ((bool)value) ? True : False;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is T && EqualityComparer<T>.Default.Equals((T)value, True);
        }
    }

    public sealed class BooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        public BooleanToVisibilityConverter() :
            base(Visibility.Visible, Visibility.Collapsed)
        { }
    }
    #endregion

    /// <summary>
    /// Interaction logic for SmoothFadeButton.xaml
    /// </summary>
    public partial class SmoothFadeButton : Button
    {
        #region Background Default
        public ImageSource BackgroundDefault
        {
            get { return (ImageSource)GetValue(BackgroundDefaultProperty); }
            set { SetValue(BackgroundDefaultProperty, value); }
        }

        public static readonly DependencyProperty BackgroundDefaultProperty =
            DependencyProperty.Register("BackgroundDefault", typeof(ImageSource), typeof(SmoothFadeButton), new PropertyMetadata(null));
        #endregion

        // -----------------------------------------------------------------------------------------------------------------------------

        #region Background Default Stretch
        public Stretch BackgroundDefaultStretch
        {
            get { return (Stretch)GetValue(BackgroundDefaultStretchProperty); }
            set { SetValue(BackgroundDefaultStretchProperty, value); }
        }

        public static readonly DependencyProperty BackgroundDefaultStretchProperty =
            DependencyProperty.Register("BackgroundDefaultStretch", typeof(Stretch), typeof(SmoothFadeButton), new PropertyMetadata(Stretch.None));
        #endregion

        // -----------------------------------------------------------------------------------------------------------------------------

        #region Background Hover
        public ImageSource BackgroundHover
        {
            get { return (ImageSource)GetValue(BackgroundHoverProperty); }
            set { SetValue(BackgroundHoverProperty, value); }
        }

        public static readonly DependencyProperty BackgroundHoverProperty =
            DependencyProperty.Register("BackgroundHover", typeof(ImageSource), typeof(SmoothFadeButton), new PropertyMetadata(null));
        #endregion

        // -----------------------------------------------------------------------------------------------------------------------------

        #region Background Hover Stretch
        public Stretch BackgroundHoverStretch
        {
            get { return (Stretch)GetValue(BackgroundHoverStretchProperty); }
            set { SetValue(BackgroundHoverStretchProperty, value); }
        }

        public static readonly DependencyProperty BackgroundHoverStretchProperty =
            DependencyProperty.Register("BackgroundHoverStretch", typeof(Stretch), typeof(SmoothFadeButton), new PropertyMetadata(Stretch.None));
        #endregion

        // -----------------------------------------------------------------------------------------------------------------------------

        #region Transition Speed
        public string TransitionSpeed
        {
            get { return (string)GetValue(TransitionSpeedProperty); }
            set { SetValue(TransitionSpeedProperty, value); }
        }

        public static readonly DependencyProperty TransitionSpeedProperty =
            DependencyProperty.Register("TransitionSpeed", typeof(string), typeof(SmoothFadeButton), new PropertyMetadata("0:0:1"));
        #endregion

        // -----------------------------------------------------------------------------------------------------------------------------

        public SmoothFadeButton()
        {
            InitializeComponent();
        }
    }
}