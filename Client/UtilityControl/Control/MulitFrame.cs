using System;
using System.Collections.Concurrent;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace UtilityControl.Control
{
    public class MulitFrame : UTabControl
    {

        private readonly ConcurrentDictionary<Uri, int> _dictionary = new ConcurrentDictionary<Uri, int>();

        static MulitFrame()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MulitFrame), new FrameworkPropertyMetadata(typeof(MulitFrame)));
        }

        public MulitFrame()
        {
            AllowAnimation = false;
            HeaderVisibility = Visibility.Collapsed;
        }

        private void View()
        {
            if (Source == null) { return; }
            if (_dictionary.ContainsKey(Source))
            {
                var selectedIndex = _dictionary[Source];
                SelectedIndex = selectedIndex;
            }
            else
            {
                var frame = new Frame { Source = Source };
                var newItem = new TabItem
                {
                    Header = Source.ToString(),
                    Content = frame
                };
                Items.Add(newItem);
                var selectedIndex = Items.Count - 1;
                SelectedIndex = selectedIndex;
                _dictionary.TryAdd(Source, selectedIndex);
            }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof(Uri), typeof(MulitFrame), new PropertyMetadata((sender, args) =>
            {
                var frame = sender as MulitFrame;
                if (frame == null) { return; }
                frame.View();
            }));

        public Uri Source
        {
            get { return (Uri)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty NavigationUIVisibilityProperty = DependencyProperty.Register(
            "NavigationUIVisibility", typeof(NavigationUIVisibility), typeof(MulitFrame), new PropertyMetadata(NavigationUIVisibility.Hidden));

        public NavigationUIVisibility NavigationUIVisibility
        {
            get { return (NavigationUIVisibility)GetValue(NavigationUIVisibilityProperty); }
            set { SetValue(NavigationUIVisibilityProperty, value); }
        }



    }
}
