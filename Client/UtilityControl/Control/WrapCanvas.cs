using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace UtilityControl.Control
{
    public class WrapCanvas : Canvas
    {

        private double _pageHeight;
        private double _pageWidth;

        public WrapCanvas()
        {
            var transform = new TranslateTransform();
            RenderTransform = transform;
        }

        private void RefreshLoyat()
        {
            PageCount = 1;
            double lineWidth = 0;
            double lineHeight = 0;
            foreach (UIElement child in InternalChildren)
            {
                if (lineWidth + child.RenderSize.Width > _pageWidth)
                {
                    lineHeight += child.RenderSize.Height;
                    lineWidth = 0;
                }
                if (lineHeight + child.RenderSize.Height > _pageHeight)
                {
                    lineHeight = 0;
                    lineWidth = 0;
                    PageCount++;
                }

                child.Arrange(new Rect((PageCount - 1) * _pageWidth + lineWidth, lineHeight,
                    child.RenderSize.Width, child.RenderSize.Height));
                lineWidth += child.RenderSize.Width;
            }
            OnLoad?.Invoke(this, null);
            CurrentPage = 0;

        }

        private void RefreshCenterLoyat()
        {
            PageCount = 1;
            double lineWidth = 0;
            double lineHeight = 0;
            var childList = new List<Tuple<string, double, UIElement>>();
            foreach (UIElement child in InternalChildren)
            {
                if (lineWidth + child.RenderSize.Width > _pageWidth)
                {
                    lineHeight += child.RenderSize.Height;
                    lineWidth = 0;
                }
                if (lineHeight + child.RenderSize.Height > _pageHeight)
                {
                    lineHeight = 0;
                    lineWidth = 0;
                    PageCount++;
                }

                child.Arrange(new Rect((PageCount - 1) * _pageWidth + lineWidth, lineHeight,
                    child.RenderSize.Width, child.RenderSize.Height));
                lineWidth += child.RenderSize.Width;
                childList.Add(new Tuple<string, double, UIElement>($"{PageCount}_{lineHeight}", lineWidth, child));
            }
            foreach (var lines in childList.GroupBy(o => o.Item1))
            {
                var offset = _pageWidth - lines.Max(o => o.Item2);
                foreach (var ui in lines)
                {
                    var trans = new TranslateTransform(offset / 2, 0);
                    ui.Item3.RenderTransform = trans;
                }
            }
            OnLoad?.Invoke(this, null);
            CurrentPage = 0;

        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            _pageHeight = arrangeSize.Height;
            _pageWidth = arrangeSize.Width;
            foreach (UIElement child in InternalChildren)
            {
                child.Arrange(new Rect(0, 0, 0, 0));
            }
            if (ContentHorizontalAlignment == ContentHorizontalAlignment.Center)
            {
                RefreshCenterLoyat();
            }
            else
            {
                RefreshLoyat();
            }

            return arrangeSize;
        }

        private void GoPage(int pageIndex)
        {
            if (pageIndex > PageCount - 1 || pageIndex < 0)
            {
                return;
            }
            DoubleAnimation moveAnimation = new DoubleAnimation
            {
                To = -(pageIndex) * ActualWidth,
                DecelerationRatio = 0.3,
                AccelerationRatio = 0.3,
                Duration = TimeSpan.FromMilliseconds(500)
            };
            RenderTransform.BeginAnimation(TranslateTransform.XProperty, moveAnimation);
        }

        #region 注册属性，事件

        public static readonly DependencyProperty CurrentPageProperty = DependencyProperty.Register(
            "CurrentPage", typeof(int), typeof(WrapCanvas), new PropertyMetadata(0, CurrentPageChanged));

        public int CurrentPage
        {
            get { return (int)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }

        private static void CurrentPageChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var wrapCanvas = sender as WrapCanvas;
            if (wrapCanvas == null) { return; }
            int currentPage = (int)e.NewValue;
            wrapCanvas.GoPage(currentPage);
        }

        public static readonly DependencyProperty PageCountProperty = DependencyProperty.Register(
            "PageCount", typeof(int), typeof(WrapCanvas), new PropertyMetadata(1));

        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            set { SetValue(PageCountProperty, value); }
        }

        public static readonly DependencyProperty ContentHorizontalAlignmentProperty = DependencyProperty.Register(
            "ContentHorizontalAlignment", typeof(ContentHorizontalAlignment), typeof(WrapCanvas), new PropertyMetadata(default(ContentHorizontalAlignment)));

        public ContentHorizontalAlignment ContentHorizontalAlignment
        {
            get { return (ContentHorizontalAlignment)GetValue(ContentHorizontalAlignmentProperty); }
            set { SetValue(ContentHorizontalAlignmentProperty, value); }
        }

        #endregion

        public event EventHandler<EventArgs> OnLoad;
    }

    public enum ContentHorizontalAlignment
    {
        Left,
        Center
    }
}
