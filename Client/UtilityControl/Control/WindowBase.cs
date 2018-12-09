using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UtilityControl.Common;
using NotifyIcon = System.Windows.Forms.NotifyIcon;

namespace UtilityControl.Control
{
    public class WindowBase : Window
    {
        #region 注册属性

        #region 默认Header：窗体字体图标UIcon

        public static readonly DependencyProperty UIconProperty =
            DependencyProperty.Register("UIcon", typeof(string), typeof(WindowBase), new PropertyMetadata(""));

        /// <summary>
        /// 按钮字体图标编码
        /// </summary>
        public string UIcon
        {
            get { return (string)GetValue(UIconProperty); }
            set { SetValue(UIconProperty, value); }
        }

        public static readonly DependencyProperty UIconForegroundProperty =
    DependencyProperty.Register("UIconForeground", typeof(Brush), typeof(WindowBase), new PropertyMetadata(Brushes.White));
        public Brush UIconForeground
        {
            get { return (Brush)GetValue(UIconForegroundProperty); }
            set { SetValue(UIconForegroundProperty, value); }
        }

        #endregion

        #region  默认Header：窗体字体图标大小

        public static readonly DependencyProperty UIconSizeProperty =
            DependencyProperty.Register("UIconSize", typeof(double), typeof(WindowBase), new PropertyMetadata(20D));

        /// <summary>
        /// 按钮字体图标大小
        /// </summary>
        public double UIconSize
        {
            get { return (double)GetValue(UIconSizeProperty); }
            set { SetValue(UIconSizeProperty, value); }
        }

        #endregion

        #region TitleHorizontalAlignment 标题位置

        public static readonly DependencyProperty TitleHorizontalAlignmentProperty =
            DependencyProperty.Register("TitleHorizontalAlignment", typeof(HorizontalAlignment), typeof(WindowBase), new PropertyMetadata(HorizontalAlignment.Left));

        /// <summary>
        /// 标题位置
        /// </summary>
        public HorizontalAlignment TitleHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(TitleHorizontalAlignmentProperty); }
            set
            {
                SetValue(TitleHorizontalAlignmentProperty, value);
            }
        }

        #endregion

        #region IconVisiblity 是否显示图标

        public static readonly DependencyProperty IconVisibilityProperty =
            DependencyProperty.Register("IconVisibility", typeof(Visibility), typeof(WindowBase), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// 是否显示图标
        /// </summary>
        public Visibility IconVisibility
        {
            get { return (Visibility)GetValue(IconVisibilityProperty); }
            set
            {
                SetValue(IconVisibilityProperty, value);
            }
        }

        #endregion

        #region CaptionHeight 标题栏高度

        public static readonly DependencyProperty CaptionHeightProperty =
            DependencyProperty.Register("CaptionHeight", typeof(double), typeof(WindowBase), new PropertyMetadata(26D));

        /// <summary>
        /// 标题高度
        /// </summary>
        public double CaptionHeight
        {
            get { return (double)GetValue(CaptionHeightProperty); }
            set
            {
                SetValue(CaptionHeightProperty, value);
                //this._WC.CaptionHeight = value;
            }
        }

        #endregion

        #region CaptionBackground 标题栏背景色

        public static readonly DependencyProperty CaptionBackgroundProperty = DependencyProperty.Register(
            "CaptionBackground", typeof(Brush), typeof(WindowBase), new PropertyMetadata(null));

        public Brush CaptionBackground
        {
            get { return (Brush)GetValue(CaptionBackgroundProperty); }
            set { SetValue(CaptionBackgroundProperty, value); }
        }

        #endregion

        #region CaptionForeground 标题栏前景景色

        public static readonly DependencyProperty CaptionForegroundProperty = DependencyProperty.Register(
            "CaptionForeground", typeof(Brush), typeof(WindowBase), new PropertyMetadata(null));

        public Brush CaptionForeground
        {
            get { return (Brush)GetValue(CaptionForegroundProperty); }
            set { SetValue(CaptionForegroundProperty, value); }
        }

        #endregion

        #region Header 标题栏内容模板，以提高默认模板，可自定义

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(ControlTemplate), typeof(WindowBase), new PropertyMetadata(null));

        public ControlTemplate Header
        {
            get { return (ControlTemplate)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        #endregion

        #region MaxboxEnable 是否显示最大化按钮

        public static readonly DependencyProperty MaxboxEnableProperty = DependencyProperty.Register(
            "MaxboxEnable", typeof(bool), typeof(WindowBase), new PropertyMetadata(true));

        public bool MaxboxEnable
        {
            get { return (bool)GetValue(MaxboxEnableProperty); }
            set { SetValue(MaxboxEnableProperty, value); }
        }

        #endregion

        #region MinboxEnable 是否显示最小化按钮

        public static readonly DependencyProperty MinboxEnableProperty = DependencyProperty.Register(
            "MinboxEnable", typeof(bool), typeof(WindowBase), new PropertyMetadata(true));

        public bool MinboxEnable
        {
            get { return (bool)GetValue(MinboxEnableProperty); }
            set { SetValue(MinboxEnableProperty, value); }
        }

        #endregion

        #region SetboxEnable 是否显示设置按钮

        public static readonly DependencyProperty SetboxEnableProperty = DependencyProperty.Register(
            "SetboxEnable", typeof(bool), typeof(WindowBase), new PropertyMetadata(false));

        public bool SetboxEnable
        {
            get { return (bool)GetValue(SetboxEnableProperty); }
            set { SetValue(SetboxEnableProperty, value); }
        }

        #endregion

        #region NotifyIconEnable 是否显示托盘图标

        public static readonly DependencyProperty NotifyIconEnableProperty = DependencyProperty.Register(
            "NotifyIconEnable", typeof(bool), typeof(WindowBase), new PropertyMetadata(false, (s, e) =>
            {
                if ((bool)e.NewValue)
                {
                    var v = s as WindowBase;
                    v?.InitialTray();
                }
            }));

        public bool NotifyIconEnable
        {
            get { return (bool)GetValue(NotifyIconEnableProperty); }
            set { SetValue(NotifyIconEnableProperty, value); }
        }

        #endregion

        #region CloseWindowCommand 关闭事件

        public static readonly DependencyProperty CloseWindowCommandProperty = DependencyProperty.Register(
            "CloseWindowCommand", typeof(ICommand), typeof(WindowBase), new PropertyMetadata());

        public ICommand CloseWindowCommand
        {
            get { return (ICommand)GetValue(CloseWindowCommandProperty); }
            set { SetValue(CloseWindowCommandProperty, value); }
        }

        #endregion

        #region ShowMask 显示遮罩层

        public static readonly DependencyProperty ShowMaskProperty = DependencyProperty.Register(
            "ShowMask", typeof(bool), typeof(WindowBase), new PropertyMetadata(false, (sender, args) =>
            {
                var window = sender as WindowBase;
                var mask = window?.GetChild<PopupExtession>("mask");
                if (mask == null)
                {
                    return;
                }
                if ((bool)args.NewValue)
                {
                    DependencyObject parent = mask.Child;
                    do
                    {
                        parent = VisualTreeHelper.GetParent(parent);
                        if (parent != null && parent.ToString() == "System.Windows.Controls.Primitives.PopupRoot")
                        {
                            var element = parent as FrameworkElement;
                            var bg = window.GetChild<Border>("Bg");

                            if (element != null)
                            {
                                element.Height = bg?.ActualHeight ?? window.ActualHeight;
                                element.Width = bg?.ActualWidth ?? window.ActualWidth;
                            }
                            break;
                        }
                    }
                    while (parent != null);
                    mask.IsOpen = true;
                }
                else
                {
                    mask.IsOpen = false;
                }

            }));

        public bool ShowMask
        {
            get { return (bool)GetValue(ShowMaskProperty); }
            set { SetValue(ShowMaskProperty, value); }
        }

        #endregion

        #endregion

        /****************** commands ******************/
        public ICommand MaximizeWindowCommand { get; protected set; }
        public ICommand MinimizeWindowCommand { get; protected set; }

        public NotifyIcon notifyIcon { get; protected set; }

        public WindowBase()
        {
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            Style = FindResource("DefaultWindowStyle") as Style;
            Uri iconUri = new Uri("pack://application:,,,/UtilityControl;component/Resource/Icon/logo_01.ico", UriKind.RelativeOrAbsolute);
            Icon = BitmapFrame.Create(iconUri);

            MaxHeight = SystemParameters.WorkArea.Height + 12 + 2;

            CloseWindowCommand = new RelayCommand(CloseCommandExecute);
            MaximizeWindowCommand = new RelayCommand(MaxCommandExecute);
            MinimizeWindowCommand = new RelayCommand(MinCommandExecute);

            Loaded += (s, e) =>
            {
                var btnSet = this.GetChild<UButton>("btnSet");
                if (btnSet != null)
                {
                    btnSet.Click += BtnSet_Click;
                }
            };

        }

        private void InitialTray()
        {

            //设置托盘的各个属性
            notifyIcon = new NotifyIcon();
            //notifyIcon.BalloonTipText = "欢迎使用星火燎原";
            notifyIcon.Text = "星火燎原";
            //Uri iconUri = new Uri("pack://application:,,,/UtilityControl;component/Resource/Icon/logo_01.ico", UriKind.RelativeOrAbsolute);
            notifyIcon.Icon = new System.Drawing.Icon(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icon", "logo_01.ico"));
            notifyIcon.Visible = true;
            //notifyIcon.ShowBalloonTip(2000);
            notifyIcon.MouseClick += notifyIcon_MouseClick;

            //设置菜单项
            //System.Windows.Forms.MenuItem menu1 = new System.Windows.Forms.MenuItem("菜单项1");
            //System.Windows.Forms.MenuItem menu2 = new System.Windows.Forms.MenuItem("菜单项2");
            //System.Windows.Forms.MenuItem menu = new System.Windows.Forms.MenuItem("菜单", new System.Windows.Forms.MenuItem[] { menu1, menu2 });

            //退出菜单项
            //System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("退出");
            //exit.Click += new EventHandler(exit_Click);

            //关联托盘控件
            //System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { menu, exit };
            //notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            //窗体状态改变时候触发
            StateChanged += SysTray_StateChanged;
        }

        private void CloseCommandExecute()
        {
            if (NotifyIconEnable)
            {
                notifyIcon.Dispose();
                Application.Current.Shutdown();
            }
            else
            {
                Close();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (NotifyIconEnable)
            {
                notifyIcon.Dispose();
            }
            base.OnClosed(e);
        }

        private void MaxCommandExecute()
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void MinCommandExecute()
        {
            WindowState = WindowState.Minimized;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void SysTray_StateChanged(object sender, EventArgs e)
        {
            //if (WindowState == WindowState.Minimized)
            //{
            //    Visibility = Visibility.Hidden;
            //}
        }

        private void exit_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("提示", "确定要退出系统吗？", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);
            if (result != MessageBoxResult.OK)
            {
                return;
            }
            notifyIcon?.Dispose();
            Application.Current.Shutdown();
        }


        private void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (WindowState == WindowState.Minimized)
                {
                    WindowState = WindowState.Normal;
                    Activate();
                }
                else
                {
                    WindowState = WindowState.Minimized;
                }
            }
        }

        private void BtnSet_Click(object sender, EventArgs e)
        {
            Setting?.Invoke(sender, e);
        }

        public event EventHandler<EventArgs> Setting;
    }
}
