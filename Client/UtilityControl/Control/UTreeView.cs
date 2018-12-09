using System.Windows;
using System.Windows.Controls;

namespace UtilityControl.Control
{
    public class UTreeView : TreeView
    {




    }

    public class UTreeViewItem : TreeViewItem
    {
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is UTreeViewItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new UTreeViewItem();
        }
    }
}
