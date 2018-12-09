using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace UtilityControl.Control
{

    [TemplatePart(Name = "loadingControl", Type = typeof(UIElement))]
    public class ULoadingDialog : UserControl
    {
        private PopupExtession _popup;

        private UIElement _loadingControl;

        private Storyboard _angleStoryboard;

        private static ULoadingDialog _loadingDialog;

        private bool _isOpen;

        public static void Show(string loadingText = "")
        {
            if (_loadingDialog == null)
            {
                _loadingDialog = new ULoadingDialog();
            }
            _loadingDialog.LoadingText = loadingText;

            if (_loadingDialog._popup == null)
            {
                _loadingDialog._popup = new PopupExtession
                {
                    PlacementTarget = Application.Current.MainWindow,
                    Placement = PlacementMode.Center,
                    PopupAnimation = PopupAnimation.None,
                    AllowsTransparency = true,
                    Child = _loadingDialog,
                    HorizontalOffset = 100
                };
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Application.Current.MainWindow.IsHitTestVisible = false;
                Application.Current.MainWindow.Opacity = 0.9;
                _loadingDialog._popup.IsOpen = false;
                _loadingDialog._popup.IsOpen = true;
                _loadingDialog._isOpen = true;
                _loadingDialog.BeginAnimation();

                Application.Current.MainWindow.SizeChanged += _loadingDialog.MainWindow_SizeChanged;
                Application.Current.MainWindow.LocationChanged += _loadingDialog.MainWindow_LocationChanged;
                Application.Current.MainWindow.Activated += _loadingDialog.MainWindow_Activated;
                Application.Current.MainWindow.Deactivated += _loadingDialog.MainWindow_Deactivated;
            });
        }

        public static void Close()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (_loadingDialog?._popup != null)
                {
                    Application.Current.MainWindow.IsHitTestVisible = true;
                    Application.Current.MainWindow.Opacity = 1;
                    _loadingDialog._popup.IsOpen = false;
                    _loadingDialog._isOpen = false;
                    _loadingDialog.StopAnimation();

                    Application.Current.MainWindow.SizeChanged -= _loadingDialog.MainWindow_SizeChanged;
                    Application.Current.MainWindow.LocationChanged -= _loadingDialog.MainWindow_LocationChanged;
                    Application.Current.MainWindow.Activated -= _loadingDialog.MainWindow_Activated;
                    Application.Current.MainWindow.Deactivated -= _loadingDialog.MainWindow_Deactivated;
                }
            });

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

        void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var mi = typeof(Popup).GetMethod("UpdatePosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            mi?.Invoke(_popup, null);
        }

        void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            var mi = typeof(Popup).GetMethod("UpdatePosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            mi?.Invoke(_popup, null);
        }

        void MainWindow_Deactivated(object sender, EventArgs e)
        {
            if (_loadingDialog != null && _loadingDialog._isOpen)
            {
                _loadingDialog._popup.IsOpen = false;
            }
        }

        void MainWindow_Activated(object sender, EventArgs e)
        {
            if (_loadingDialog != null && _loadingDialog._isOpen)
            {
                _loadingDialog._popup.IsOpen = false;
                _loadingDialog._popup.IsOpen = true;
            }
        }

        static ULoadingDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ULoadingDialog), new FrameworkPropertyMetadata(typeof(ULoadingDialog)));
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

        }

        public static readonly DependencyProperty LoadingTextProperty = DependencyProperty.Register(
            "LoadingText", typeof(string), typeof(ULoadingDialog), new PropertyMetadata(""));

        public string LoadingText
        {
            get { return (string)GetValue(LoadingTextProperty); }
            set { SetValue(LoadingTextProperty, value); }
        }

        public static readonly DependencyProperty AllowRotationProperty = DependencyProperty.Register(
            "AllowRotation", typeof(bool), typeof(ULoadingDialog), new PropertyMetadata(true));

        public bool AllowRotation
        {
            get { return (bool)GetValue(AllowRotationProperty); }
            set { SetValue(AllowRotationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation", typeof(Orientation), typeof(ULoadingDialog), new PropertyMetadata(Orientation.Horizontal));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

    }
}
