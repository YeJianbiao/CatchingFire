using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UtilityControl.Control
{
    public class UButton:Button
    {
        public static readonly DependencyProperty PressedBackgroundProperty =
            DependencyProperty.Register("PressedBackground", typeof(Brush), typeof(UButton), new PropertyMetadata(Brushes.DarkBlue));
        /// <summary>
        /// 鼠标按下背景样式
        /// </summary>
        public Brush PressedBackground
        {
            get { return (Brush)GetValue(PressedBackgroundProperty); }
            set { SetValue(PressedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty PressedForegroundProperty =
            DependencyProperty.Register("PressedForeground", typeof(Brush), typeof(UButton), new PropertyMetadata(Brushes.White));
        /// <summary>
        /// 鼠标按下前景样式（图标、文字）
        /// </summary>
        public Brush PressedForeground
        {
            get { return (Brush)GetValue(PressedForegroundProperty); }
            set { SetValue(PressedForegroundProperty, value); }
        }

        public static readonly DependencyProperty MouseOverBackgroundProperty =
            DependencyProperty.Register("MouseOverBackground", typeof(Brush), typeof(UButton), new PropertyMetadata(Brushes.RoyalBlue));
        /// <summary>
        /// 鼠标进入背景样式
        /// </summary>
        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }

        public static readonly DependencyProperty MouseOverForegroundProperty =
            DependencyProperty.Register("MouseOverForeground", typeof(Brush), typeof(UButton), new PropertyMetadata(Brushes.White));
        /// <summary>
        /// 鼠标进入前景样式
        /// </summary>
        public Brush MouseOverForeground
        {
            get { return (Brush)GetValue(MouseOverForegroundProperty); }
            set { SetValue(MouseOverForegroundProperty, value); }
        }

        public static readonly DependencyProperty UIconProperty =
            DependencyProperty.Register("UIcon", typeof(string), typeof(UButton), new PropertyMetadata(""));
        /// <summary>
        /// 按钮字体图标编码
        /// </summary>
        public string UIcon
        {
            get { return (string)GetValue(UIconProperty); }
            set { SetValue(UIconProperty, value); }
        }

        public static readonly DependencyProperty UIconSizeProperty =
            DependencyProperty.Register("UIconSize", typeof(int), typeof(UButton), new PropertyMetadata(20));
        /// <summary>
        /// 按钮字体图标大小
        /// </summary>
        public int UIconSize
        {
            get { return (int)GetValue(UIconSizeProperty); }
            set { SetValue(UIconSizeProperty, value); }
        }

        public static readonly DependencyProperty UIconMarginProperty = DependencyProperty.Register(
            "UIconMargin", typeof(Thickness), typeof(UButton), new PropertyMetadata(new Thickness(0, 1, 3, 1)));
        /// <summary>
        /// 字体图标间距
        /// </summary>
        public Thickness UIconMargin
        {
            get { return (Thickness)GetValue(UIconMarginProperty); }
            set { SetValue(UIconMarginProperty, value); }
        }

        public static readonly DependencyProperty AllowsAnimationProperty = DependencyProperty.Register(
            "AllowsAnimation", typeof(bool), typeof(UButton), new PropertyMetadata(true));
        /// <summary>
        /// 是否启用UIcon动画
        /// </summary>
        public bool AllowsAnimation
        {
            get { return (bool)GetValue(AllowsAnimationProperty); }
            set { SetValue(AllowsAnimationProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UButton), new PropertyMetadata(new CornerRadius(2)));
        /// <summary>
        /// 按钮圆角大小,左上，右上，右下，左下
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty ContentDecorationsProperty = DependencyProperty.Register(
            "ContentDecorations", typeof(TextDecorationCollection), typeof(UButton), new PropertyMetadata(null));
        public TextDecorationCollection ContentDecorations
        {
            get { return (TextDecorationCollection)GetValue(ContentDecorationsProperty); }
            set { SetValue(ContentDecorationsProperty, value); }
        }

        static UButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UButton), new FrameworkPropertyMetadata(typeof(UButton)));
        }
    }
}
