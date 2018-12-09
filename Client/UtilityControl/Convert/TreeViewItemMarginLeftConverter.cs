using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using UtilityControl.Control;

namespace UtilityControl.Convert
{
    public class TreeViewItemMarginLeftConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double columnWidth;
            bool flag = double.TryParse(parameter + "", out columnWidth);
            if (!flag)
            {
                columnWidth = 15.0;
            }
            var left = 0.0;
            UIElement element = value as TreeViewItem;

            while (element != null && element.GetType() != typeof(UTreeView) && element.GetType() != typeof(TreeView) &&
                   element.GetType() != typeof(UTreeListView))
            {
                element = (UIElement)VisualTreeHelper.GetParent(element);
                if (element != null)
                {
                    if (element.GetType() == typeof(TreeViewItem) ||
                        element.GetType() == typeof(UTreeListViewItem))
                    {
                        left += columnWidth;
                    }
                }
            }
            return new Thickness(left, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
