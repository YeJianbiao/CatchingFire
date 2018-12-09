using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace UtilityControl.Control
{
    public class UKeyTextBlock : TextBlock
    {

        static UKeyTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UKeyTextBlock), new FrameworkPropertyMetadata(typeof(UKeyTextBlock)));
        }

        public UKeyTextBlock()
        {
            Loaded += (sender, args) =>
            {
                RecombinationText();
            };
        }

        private void RecombinationText()
        {
            bool startsWithKey = Text.StartsWith(Key.Trim());
            bool endWithKey = Text.EndsWith(Key);
            var strings = Text.Split(new[] { Key }, StringSplitOptions.RemoveEmptyEntries).Where(o => !string.IsNullOrEmpty(o)).ToList();
            Inlines.Clear();
            if (startsWithKey)
            {
                var run = new Run
                {
                    Text = Key,
                    Foreground = KeyBrush
                };
                Inlines.Add(run);
            }
            for (var i = 0; i < strings.Count; i++)
            {
                var run = new Run { Text = strings[i] };
                Inlines.Add(run);
                if (i < strings.Count - 1)
                {
                    var runKey = new Run
                    {
                        Text = Key,
                        Foreground = KeyBrush
                    };
                    Inlines.Add(runKey);
                }
            }
            if (endWithKey)
            {
                var run = new Run
                {
                    Text = Key,
                    Foreground = KeyBrush
                };
                Inlines.Add(run);
            }
        }

        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(
            "Key", typeof(string), typeof(UKeyTextBlock), new PropertyMetadata((o, args) =>
          {
              var textBlock = o as UKeyTextBlock;
              if (textBlock == null) { return; }
              if (!textBlock.IsLoaded) { return; }
              textBlock.RecombinationText();

          }));

        public string Key
        {
            get { return (string)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        public static readonly DependencyProperty KeyBrushProperty = DependencyProperty.Register(
            "KeyBrush", typeof(Brush), typeof(UKeyTextBlock), new PropertyMetadata(Brushes.Red));

        public Brush KeyBrush
        {
            get { return (Brush)GetValue(KeyBrushProperty); }
            set { SetValue(KeyBrushProperty, value); }
        }

    }
}
