using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using UtilityControl.Common;


namespace UtilityControl.Control
{
    public class UTabControl : TabControl
    {
        private int _oldSelectedIndex;

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            var gContent = this.GetChild<Grid>("gContent");
            if (gContent == null) { return; }
            if (gContent.Children.Cast<ContentPresenter>().Count(o=>o.Name== $"item_{SelectedIndex}" )== 0)
            {
                var contentPresenter = new ContentPresenter
                {
                    Name = $"item_{SelectedIndex}",
                    Content = SelectedContent,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch
                };
                gContent.Children.Add(contentPresenter);
            }
            
                if (SelectedIndex != _oldSelectedIndex)
                {
                var child =gContent.Children.Cast<ContentPresenter>()
                                .First(o => o.Name == $"item_{SelectedIndex}");
                var oldChild = gContent.Children.Cast<ContentPresenter>()
                                .First(o => o.Name == $"item_{_oldSelectedIndex}");
                if (Orientation == Orientation.Horizontal)
                    {
                        HorizontalSocroll(oldChild, 0,
                            SelectedIndex > _oldSelectedIndex ? -ActualWidth : ActualWidth);
                        HorizontalSocroll(child,
                            SelectedIndex > _oldSelectedIndex ? ActualWidth : -ActualWidth, 0);
                    }
                    else
                    {
                        
                        VerticalSocroll(oldChild, 0,
                            SelectedIndex > _oldSelectedIndex ? -ActualHeight : ActualHeight);
                        VerticalSocroll(child,
                            SelectedIndex > _oldSelectedIndex ? ActualHeight : -ActualHeight, 0);
                }
            }

                _oldSelectedIndex = SelectedIndex;

        }

        private void HorizontalSocroll(UIElement ui, double form, double to, double durationMIlliseconds = 300)
        {
            var animation = new DoubleAnimation
            {
                From = form,
                To = to,
                DecelerationRatio = 0.3,
                AccelerationRatio = 0.3,
                Duration = TimeSpan.FromMilliseconds(AllowAnimation ? durationMIlliseconds : 0)
            };
            if (!(ui.RenderTransform is TranslateTransform))
            {
                ui.RenderTransform = new TranslateTransform();
            }
            ui.RenderTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        private void VerticalSocroll(UIElement ui, double form, double to, double durationMIlliseconds = 300)
        {
            var animation = new DoubleAnimation
            {
                From = form,
                To = to,
                DecelerationRatio = 0.3,
                AccelerationRatio = 0.3,
                Duration = TimeSpan.FromMilliseconds(AllowAnimation ? durationMIlliseconds : 0)
            };
            if (!(ui.RenderTransform is TranslateTransform))
            {
                ui.RenderTransform = new TranslateTransform();
            }
            ui.RenderTransform.BeginAnimation(TranslateTransform.YProperty, animation);
        }

        public static readonly DependencyProperty AllowAnimationProperty = DependencyProperty.Register(
            "AllowAnimation", typeof(bool), typeof(UTabControl), new PropertyMetadata(true));

        public bool AllowAnimation
        {
            get { return (bool)GetValue(AllowAnimationProperty); }
            set { SetValue(AllowAnimationProperty, value); }
        }

        public static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.Register(
            "HeaderVisibility", typeof (Visibility), typeof (UTabControl), new PropertyMetadata(Visibility.Visible));

        public Visibility HeaderVisibility
        {
            get { return (Visibility) GetValue(HeaderVisibilityProperty); }
            set { SetValue(HeaderVisibilityProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation", typeof(Orientation), typeof(UTabControl), new PropertyMetadata(Orientation.Horizontal));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
    }
}