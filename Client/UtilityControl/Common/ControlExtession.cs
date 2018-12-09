using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace UtilityControl.Common
{
    public static class ControlExtession
    {

        #region 语言翻译

        /// <summary>
        /// 遍历控件，对控件重新进行语言翻译
        /// </summary>
        /// <param name="uiElement"></param>
        public static void LanguageTranslation(this DependencyObject uiElement)
        {
            uiElement.SetLanguageValue();
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(uiElement);
            while (queue.Count > 0)
            {
                var child = queue.Dequeue();
                if (child is System.Windows.Controls.Primitives.Popup)
                {
                    queue.Enqueue(((System.Windows.Controls.Primitives.Popup)(child)).Child);
                    continue;
                }

                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(child); i++)
                {
                    var current = VisualTreeHelper.GetChild(child, i);
                    queue.Enqueue(current);
                    current.SetLanguageValue();
                }

            }
        }

        public static void SetLanguageValue(this DependencyObject uiElement)
        {
            string languageText = uiElement.GetValue(ControlAttachProperty.LanguageTextProperty) as string;
            if (string.IsNullOrEmpty(languageText)) { return; }
            var value = languageText.GetLanguageText();
            var content = string.IsNullOrEmpty(value) ? languageText : value;
            if (uiElement is TextBlock)
            {
                (uiElement as TextBlock).Text = content;
            }
            else if (uiElement is Label)
            {
                (uiElement as Label).Content = content ;
            }
            else if (uiElement is Button)
            {
                (uiElement as Button).Content = content ;
            }
            else if (uiElement is CheckBox)
            {
                (uiElement as CheckBox).Content = content ;
            }
            else if (uiElement is RadioButton)
            {
                (uiElement as RadioButton).Content = content ;
            }
            else if (uiElement is Run)
            {
                (uiElement as Run).Text = content ;
            }
            else if (uiElement is Window)
            {
                (uiElement as Window).Title = content ;
            }
        }

        /// <summary>
        /// 获取翻译
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetLanguageText(this string key)
        {
            if (key == null)
            {
                return string.Empty;
            }
            var v = Application.Current.TryFindResource(key);
            return v?.ToString() ?? string.Empty;
        }

        #endregion

        #region

        public static T GetChild<T>(this DependencyObject obj, string childName = "") where T : class
        {
            if (obj == null) { return default(T); }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && (child.GetValue(FrameworkElement.NameProperty) + "" == childName || string.IsNullOrEmpty(childName)))
                {
                    return child as T;
                }
                var grandChild = GetChild<T>(child, childName);
                if (grandChild != null)
                {
                    return grandChild;
                }

            }
            return default(T);
        }

        #endregion

        #region TreeView操作扩展方法
        //code:http://www.codeproject.com/Articles/36193/WPF-TreeView-tools

        /// <summary>
        /// Returns the TreeViewItem of a data bound object.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        /// <returns>The TreeViewItem of the data bound object or null.</returns>
        public static TreeViewItem GetItemFromObject(this TreeView treeView, object obj)
        {
            try
            {
                DependencyObject dObject = GetContainerFormObject(treeView, obj);
                TreeViewItem tvi = dObject as TreeViewItem;
                while (tvi == null)
                {
                    dObject = VisualTreeHelper.GetParent(dObject);
                    tvi = dObject as TreeViewItem;
                }
                return tvi;
            }
            catch
            {
            }
            return null;
        }

        private static DependencyObject GetContainerFormObject(ItemsControl item, object obj)
        {
            if (item == null)
                return null;

            DependencyObject dObject = null;
            dObject = item.ItemContainerGenerator.ContainerFromItem(obj);

            if (dObject != null)
                return dObject;

            var query = from childItem in item.Items.Cast<object>()
                        let childControl = item.ItemContainerGenerator.ContainerFromItem(childItem) as ItemsControl
                        select GetContainerFormObject(childControl, obj);

            return query.FirstOrDefault(i => i != null);
        }

        /// <summary>
        /// Selects a data bound object of a TreeView.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        public static void SelectObject(this TreeView treeView, object obj)
        {
            treeView.SelectObject(obj, true);
        }

        /// <summary>
        /// Selects or deselects a data bound object of a TreeView.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        /// <param name="selected">select or deselect</param>
        public static void SelectObject(this TreeView treeView, object obj, bool selected)
        {
            var tvi = treeView.GetItemFromObject(obj);
            if (tvi != null)
            {
                tvi.IsSelected = selected;
            }
        }

        /// <summary>
        /// Returns if a data bound object of a TreeView is selected.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        /// <returns>Returns true if the object is selected, and false if it is not selected or obj is not in the tree.</returns>
        public static bool IsObjectSelected(this TreeView treeView, object obj)
        {
            var tvi = treeView.GetItemFromObject(obj);
            if (tvi != null)
            {
                return tvi.IsSelected;
            }
            return false;
        }

        /// <summary>
        /// Returns if a data bound object of a TreeView is focused.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        /// <returns>Returns true if the object is focused, and false if it is not focused or obj is not in the tree.</returns>
        public static bool IsObjectFocused(this TreeView treeView, object obj)
        {
            var tvi = treeView.GetItemFromObject(obj);
            if (tvi != null)
            {
                return tvi.IsFocused;
            }
            return false;
        }

        /// <summary>
        /// Expands a data bound object of a TreeView.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        public static void ExpandObject(this TreeView treeView, object obj)
        {
            treeView.ExpandObject(obj, true);
        }

        /// <summary>
        /// Expands or collapses a data bound object of a TreeView.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        /// <param name="expanded">expand or collapse</param>
        public static void ExpandObject(this TreeView treeView, object obj, bool expanded)
        {
            var tvi = treeView.GetItemFromObject(obj);
            if (tvi != null)
            {
                tvi.IsExpanded = expanded;
                if (expanded)
                {
                    // update layout, so that following calls to f.e. SelectObject on child nodes will 
                    // find theire TreeViewNodes
                    treeView.UpdateLayout();
                }
            }
        }

        /// <summary>
        /// Returns if a douta bound object of a TreeView is expanded.
        /// </summary>
        /// <param name="treeView">TreeView</param>
        /// <param name="obj">Data bound object</param>
        /// <returns>Returns true if the object is expanded, and false if it is collapsed or obj is not in the tree.</returns>
        public static bool IsObjectExpanded(this TreeView treeView, object obj)
        {
            var tvi = treeView.GetItemFromObject(obj);
            if (tvi != null)
            {
                return tvi.IsExpanded;
            }
            return false;
        }

        /// <summary>
        /// Retuns the parent TreeViewItem.
        /// </summary>
        /// <param name="item">TreeViewItem</param>
        /// <returns>Parent TreeViewItem</returns>
        public static TreeViewItem GetParentItem(this TreeViewItem item)
        {
            var dObject = VisualTreeHelper.GetParent(item);
            TreeViewItem tvi = dObject as TreeViewItem;
            while (tvi == null)
            {
                dObject = VisualTreeHelper.GetParent(dObject);
                tvi = dObject as TreeViewItem;
            }
            return tvi;
        }

        #endregion

    }

}
