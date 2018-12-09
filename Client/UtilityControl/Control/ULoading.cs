using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace UtilityControl.Control
{
    [TemplatePart(Name = "loadingControl", Type = typeof(UIElement))]
    public class ULoading : UserControl
    {

        private UIElement _loadingControl;

        private Storyboard _angleStoryboard;

        static ULoading()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ULoading), new FrameworkPropertyMetadata(typeof(ULoading)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _loadingControl = GetTemplateChild("loadingControl") as UIElement;
            if (AllowRotation && _loadingControl != null)
            {
                RotateTransform rtf = new RotateTransform();
                _loadingControl.RenderTransform = rtf;
                _loadingControl.RenderTransformOrigin = new Point(0.5, 0.5);
                DoubleAnimation angleAnimation = new DoubleAnimation()
                {
                    From = 0,
                    To = 360,
                    Duration = new Duration(TimeSpan.FromMilliseconds(2000)),
                    RepeatBehavior = RepeatBehavior.Forever
                };
                _angleStoryboard = new Storyboard();
                _angleStoryboard.Children.Add(angleAnimation);
                Storyboard.SetTarget(angleAnimation, _loadingControl);
                Storyboard.SetTargetProperty(angleAnimation, new PropertyPath("RenderTransform.Angle"));
            }
            if (IsLoading)
            {
                BeginAnimation();
            }
        }

        private void BeginAnimation()
        {
            if (AllowRotation)
            {
                _angleStoryboard?.Begin(this);
            }
        }

        private void StopAnimation()
        {
            if (AllowRotation)
            {
                _angleStoryboard?.Stop(this);
            }
        }

        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register(
            "IsLoading", typeof(bool), typeof(ULoading), new PropertyMetadata(((o, args) =>
            {
                var uLoading = o as ULoading;
                if (uLoading == null) { return; }
                var isLoading = (bool)args.NewValue;
                if (isLoading) { uLoading.BeginAnimation(); } else { uLoading.StopAnimation(); }
            })));

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty LoadingTextProperty = DependencyProperty.Register(
            "LoadingText", typeof(string), typeof(ULoading), new PropertyMetadata(""));

        public string LoadingText
        {
            get { return (string)GetValue(LoadingTextProperty); }
            set { SetValue(LoadingTextProperty, value); }
        }

        public static readonly DependencyProperty AllowRotationProperty = DependencyProperty.Register(
            "AllowRotation", typeof(bool), typeof(ULoading), new PropertyMetadata(true));

        public bool AllowRotation
        {
            get { return (bool)GetValue(AllowRotationProperty); }
            set { SetValue(AllowRotationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation", typeof(Orientation), typeof(ULoading), new PropertyMetadata(Orientation.Horizontal));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

    }
}
