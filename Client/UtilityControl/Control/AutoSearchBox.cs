using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UtilityControl.Common;

namespace UtilityControl.Control
{
    [TemplatePart(Name = "popupFilter", Type = typeof(PopupExtession))]
    [TemplatePart(Name = "listBoxFilter", Type = typeof(ListBox))]
    [TemplatePart(Name = "btnSearch", Type = typeof(UButton))]
    public class AutoSearchBox : TextBox
    {
        private PopupExtession _popupFilter;

        private ListBox _listBoxFilter;

        private UButton _btnSearch;

        private bool _fuzzyEnable = true;

        static AutoSearchBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoSearchBox), new FrameworkPropertyMetadata(typeof(AutoSearchBox)));
        }

        public AutoSearchBox()
        {
            Loaded += (sender, args) =>
            {
                if (FuzzySearch == null)
                {
                    var method = DataContext?.GetType().GetProperty(FuzzySearchMethodName)?.GetValue(DataContext);
                    if (method != null)
                    {
                        SetValue(FuzzySearchProperty, method);
                    }
                }

                _btnSearch.Click += AutoSearch_ButtonClick;
                _listBoxFilter.SelectionChanged += FilterSelectionChanged;
            };

            Unloaded += (sender, args) =>
            {
                _btnSearch.Click -= AutoSearch_ButtonClick;
                _listBoxFilter.SelectionChanged -= FilterSelectionChanged;
            };
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if (!_fuzzyEnable) { return; }
            _keySelectedIndex = -1;
            if (Text.Length < MinTextLenght)
            {
                _listBoxFilter.ItemsSource = null;
                _popupFilter.IsOpen = false;
                return;
            }
            FuzzySearch?.BeginInvoke(Text, o =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    var result = FuzzySearch.EndInvoke(o);
                    if (result.Item1 == Text)
                    {
                        var source = result.Item2 as IEnumerable;
                        _listBoxFilter.ItemsSource = source;
                        _popupFilter.IsOpen = source != null;

                    }
                }));

            }, null);
        }

        private int _keySelectedIndex = -1;

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            _fuzzyEnable = true;
            if (e.Key == Key.Enter)
            {
                Search?.Invoke(this, e);
                Command?.Execute(CommandParameter);
                _popupFilter.IsOpen = false;
            }
            else if (e.Key == Key.Down)
            {
                if (_keySelectedIndex < _listBoxFilter.Items.Count)
                {
                    _keySelectedIndex++;
                }
                if (_keySelectedIndex >= _listBoxFilter.Items.Count)
                {
                    _keySelectedIndex = 0;
                }
                KeySelectedItem();
            }
            else if (e.Key == Key.Up)
            {
                if (_keySelectedIndex >= 0)
                {
                    _keySelectedIndex--;
                }
                if (_keySelectedIndex < 0)
                {
                    _keySelectedIndex = _listBoxFilter.Items.Count - 1;
                }
                KeySelectedItem();
            }
        }

        private void AutoSearch_ButtonClick(object sender, RoutedEventArgs e)
        {
            Search?.Invoke(sender, e);
            Command?.Execute(CommandParameter);
        }

        private void KeySelectedItem()
        {
            foreach (var item in _listBoxFilter.Items)
            {
                var control = _listBoxFilter.ItemContainerGenerator.ContainerFromItem(item);
                var g = ControlHelper.GetChild<Grid>(control);
                g?.ClearValue(BackgroundProperty);
            }
            var itemControl = _listBoxFilter.ItemContainerGenerator.ContainerFromIndex(_keySelectedIndex);
            var ui = ControlHelper.GetChild<Grid>(itemControl);
            if (ui != null)
            {
                ui.Background = FindResource("SearchItemMouseOverBackground") as SolidColorBrush;
            }
            _fuzzyEnable = false;
            Text = _listBoxFilter.Items[_keySelectedIndex].ToString();
            if (Text != null)
            {
                Select(Text.Length, 0);
            }
        }

        private void FilterSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox?.SelectedItem == null) { return; }
            _popupFilter.IsOpen = false;
            _fuzzyEnable = false;
            Text = listBox.SelectedItem.ToString();
            Focus();
            if (Text != null)
            {
                Select(Text.Length, 0);
            }
        }

        public event EventHandler<EventArgs> Search;


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _popupFilter = GetTemplateChild("popupFilter") as PopupExtession;
            _listBoxFilter = GetTemplateChild("listBoxFilter") as ListBox;
            _btnSearch = GetTemplateChild("btnSearch") as UButton;
        }

        public static readonly DependencyProperty SearchButtonEnabledProperty = DependencyProperty.Register(
            "SearchButtonEnabled", typeof(bool), typeof(AutoSearchBox), new PropertyMetadata(true));

        public bool SearchButtonEnabled
        {
            get { return (bool)GetValue(SearchButtonEnabledProperty); }
            set { SetValue(SearchButtonEnabledProperty, value); }
        }

        public static readonly DependencyProperty FuzzySearchMethodNameProperty = DependencyProperty.Register(
            "FuzzySearchMethodName", typeof(string), typeof(AutoSearchBox), new PropertyMetadata(default(string)));

        public string FuzzySearchMethodName
        {
            get { return (string)GetValue(FuzzySearchMethodNameProperty); }
            set { SetValue(FuzzySearchMethodNameProperty, value); }
        }

        public static readonly DependencyProperty MinTextLenghtProperty = DependencyProperty.Register(
            "MinTextLenght", typeof(int), typeof(AutoSearchBox), new PropertyMetadata(2));

        public int MinTextLenght
        {
            get { return (int)GetValue(MinTextLenghtProperty); }
            set { SetValue(MinTextLenghtProperty, value); }
        }

        public static readonly DependencyProperty FuzzySearchProperty = DependencyProperty.Register(
            "FuzzySearch", typeof(Func<string, Tuple<string, object>>), typeof(AutoSearchBox), new PropertyMetadata(null));

        public Func<string, Tuple<string, object>> FuzzySearch
        {
            get { return (Func<string, Tuple<string, object>>)GetValue(FuzzySearchProperty); }
            set { SetValue(FuzzySearchProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof(ICommand), typeof(AutoSearchBox), new PropertyMetadata(default(ICommand)));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter", typeof(object), typeof(AutoSearchBox), new PropertyMetadata(default(object)));

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

    }
}
