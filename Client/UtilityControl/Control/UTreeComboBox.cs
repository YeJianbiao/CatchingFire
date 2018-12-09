using System.Collections;
using System.Windows;
using System.Windows.Controls;
using UtilityControl.Model;

namespace UtilityControl.Control
{
    [TemplatePart(Name = "PART_tree", Type = typeof(TreeView))]
    public class UTreeComboBox : ComboBox
    {
        private TreeView _treeView;

        static UTreeComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UTreeComboBox), new FrameworkPropertyMetadata(typeof(UTreeComboBox)));
        }

        public UTreeComboBox()
        {
            Loaded += (sender, args) =>
            {
                _treeView.SelectedItemChanged += TreeView_SelectionChanged;
            };
            Unloaded += (sender, args) =>
            {
                _treeView.SelectedItemChanged -= TreeView_SelectionChanged;
            };
        }

        private void TreeView_SelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = e.NewValue;
            IsDropDownOpen = false;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _treeView = GetTemplateChild("PART_tree") as TreeView;
        }

        public static readonly DependencyProperty TreeSourceProperty = DependencyProperty.Register(
            "TreeSource", typeof(IEnumerable), typeof(UTreeComboBox), new PropertyMetadata((o, args) =>
          {
              var box = o as UTreeComboBox;
              if (box == null) { return; }
              if (!box.IsLoaded) { return; }
              var nodes = args.NewValue as IEnumerable;
              if (nodes == null) { return; }
              foreach (var node in nodes)
              {
                  box.InitComboBoxItems(node as TreeComboBoxNode);
              }
          }));

        public IEnumerable TreeSource
        {
            get { return (IEnumerable)GetValue(TreeSourceProperty); }
            set { SetValue(TreeSourceProperty, value); }
        }

        private void InitComboBoxItems(TreeComboBoxNode node)
        {
            if (node.Nodes == null) { return; }
            foreach (var n in node.Nodes)
            {
                Items.Add(n);
                InitComboBoxItems(n);
            }
        }
    }
}
