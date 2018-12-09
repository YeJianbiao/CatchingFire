using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UtilityControl.Control
{
    public class UTreeListView : TreeView
    {

        public UTreeListView()
        {
            Columns = new GridViewColumnCollection();
            OperationColumns = new GridViewColumnCollection();
            EditColumns = new GridViewColumnCollection();
            EditOperationColumns = new GridViewColumnCollection();
        }

        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(
            "Columns", typeof (GridViewColumnCollection), typeof (UTreeListView), new PropertyMetadata(default(GridViewColumnCollection)));

        public GridViewColumnCollection Columns
        {
            get { return (GridViewColumnCollection) GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        public static readonly DependencyProperty OperationColumnsProperty = DependencyProperty.Register(
            "OperationColumns", typeof (GridViewColumnCollection), typeof (UTreeListView), new PropertyMetadata(default(GridViewColumnCollection)));

        public GridViewColumnCollection OperationColumns
        {
            get { return (GridViewColumnCollection) GetValue(OperationColumnsProperty); }
            set { SetValue(OperationColumnsProperty, value); }
        }

        public static readonly DependencyProperty EditColumnsProperty = DependencyProperty.Register(
            "EditColumns", typeof (GridViewColumnCollection), typeof (UTreeListView), new PropertyMetadata(default(GridViewColumnCollection)));

        public GridViewColumnCollection EditColumns
        {
            get { return (GridViewColumnCollection) GetValue(EditColumnsProperty); }
            set { SetValue(EditColumnsProperty, value); }
        }

        public static readonly DependencyProperty EditOperationColumnsProperty = DependencyProperty.Register(
            "EditOperationColumns", typeof (GridViewColumnCollection), typeof (UTreeListView), new PropertyMetadata(default(GridViewColumnCollection)));

        public GridViewColumnCollection EditOperationColumns
        {
            get { return (GridViewColumnCollection) GetValue(EditOperationColumnsProperty); }
            set { SetValue(EditOperationColumnsProperty, value); }
        }

        public static readonly DependencyProperty AllowsColumnReorderProperty = DependencyProperty.Register(
            "AllowsColumnReorder", typeof (bool), typeof (UTreeListView), new PropertyMetadata(false));

        public bool AllowsColumnReorder
        {
            get { return (bool) GetValue(AllowsColumnReorderProperty); }
            set { SetValue(AllowsColumnReorderProperty, value); }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new UTreeListViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is UTreeListViewItem;
        }

    }

    public class UTreeListViewItem : TreeViewItem
    {
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is UTreeListViewItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new UTreeListViewItem();
        }


        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);
            IsSelected = false;
            e.Handled = false;
        }

        //protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        //{
        //    base.OnMouseDoubleClick(e);
        //    IsEdit = !IsEdit;
        //    e.Handled = true;
        //}

        public static readonly DependencyProperty IsEditProperty = DependencyProperty.Register(
            "IsEdit", typeof (bool), typeof (UTreeListViewItem), new PropertyMetadata(default(bool)));

        public bool IsEdit
        {
            get { return (bool) GetValue(IsEditProperty); }
            set { SetValue(IsEditProperty, value); }
        }


    }
}
