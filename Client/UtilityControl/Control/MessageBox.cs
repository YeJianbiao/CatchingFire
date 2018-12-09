using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace UtilityControl.Control
{
    [TemplatePart(Name = "PART_CtrlButtonCollection", Type = typeof(ItemsControl))]
    public sealed class MessageBox : WindowBase
    {

        public static string Ok = "Ok";
        public static string Cancel = "Cancel";
        public static string Yes = "Yes";
        public static string No = "No";

        public static readonly DependencyProperty MessageProperty =
    DependencyProperty.Register("Message", typeof(string), typeof(MessageBox), new PropertyMetadata(""));

        public static readonly DependencyProperty CtrlButtonCollectionProperty =
    DependencyProperty.Register("CtrlButtonCollection", typeof(ObservableCollection<UButton>), typeof(MessageBox), new PropertyMetadata(new ObservableCollection<UButton>()));


        static MessageBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageBox), new FrameworkPropertyMetadata(typeof(MessageBox)));
        }

        public MessageBox()
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Style = FindResource("DefaultMessageBoxStyle") as Style;
            AllowsTransparency = true;
            WindowStyle = WindowStyle.None;
            ShowInTaskbar = true;
            Topmost = false;
            Loaded += (sender, args) =>
            {
                if (Owner == null || Owner.Equals(Application.Current.MainWindow))
                {
                    if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
                    {
                        Left = (Application.Current.MainWindow.ActualWidth - 200 - ActualWidth) / 2 + 200;
                        Top = (Application.Current.MainWindow.ActualHeight - ActualHeight) / 2;
                    }
                    else
                    {
                        Left = Application.Current.MainWindow.Left + (Application.Current.MainWindow.ActualWidth - 200 - ActualWidth) / 2 + 200;
                        Top = Application.Current.MainWindow.Top + (Application.Current.MainWindow.ActualHeight - ActualHeight) / 2;
                    }
                }
            };
        }


        public static MessageBoxResult Show(string messageBoxText)
        {
            return Show(messageBoxText, "", MessageBoxButton.OK);
        }

        public static MessageBoxResult Show(string caption, string messageBoxText)
        {
            return Show(caption, messageBoxText, MessageBoxButton.OK);
        }

        public static MessageBoxResult Show(string caption, string messageBoxText, MessageBoxButton button)
        {
            return Show(caption, messageBoxText, button, null);
        }

        public static MessageBoxResult Show(string caption, string messageBoxText, MessageBoxButton button, Window owner)
        {
            MessageBoxResult defRsult = MessageBoxResult.None;
            switch (button)
            {
                case MessageBoxButton.OK:
                    defRsult = MessageBoxResult.OK;
                    break;
                case MessageBoxButton.OKCancel:
                    defRsult = MessageBoxResult.Cancel;
                    break;
                case MessageBoxButton.YesNo:
                    defRsult = MessageBoxResult.No;
                    break;
                case MessageBoxButton.YesNoCancel:
                    defRsult = MessageBoxResult.Cancel;
                    break;
                default:
                    break;
            }

            return Show(caption, messageBoxText, button, MessageBoxImage.None, defRsult, owner);
        }

        public static MessageBoxResult Show(string caption, string messageBoxText, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult)
        {
            return Show(caption, messageBoxText, button, icon, defaultResult, null);
        }

        public static MessageBoxResult Show(string caption, string messageBoxText, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult, Window owner)
        {
            var mbox = new MessageBox
            {
                Message = messageBoxText,
                Title = caption,
                Owner = owner
            };

            if (owner != null)
            {
                mbox.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            #region
            switch (button)
            {
                case MessageBoxButton.OKCancel:
                    mbox.CtrlButtonCollection.Add(CreateCtrlButtonWithResult(mbox, Ok, true, null, defaultResult == MessageBoxResult.OK));
                    mbox.CtrlButtonCollection.Add(CreateCtrlButtonWithResult(mbox, Cancel, false, null, defaultResult == MessageBoxResult.Cancel));
                    break;
                case MessageBoxButton.YesNo:
                    mbox.CtrlButtonCollection.Add(CreateCtrlButtonWithResult(mbox, Yes, true, null, defaultResult == MessageBoxResult.Yes));

                    mbox.CtrlButtonCollection.Add(CreateCtrlButtonWithResult(mbox, No, false, null, defaultResult == MessageBoxResult.No));

                    break;
                case MessageBoxButton.YesNoCancel:
                    mbox.CtrlButtonCollection.Add(CreateCtrlButtonWithResult(mbox, Yes, true, null, defaultResult == MessageBoxResult.Yes));

                    mbox.CtrlButtonCollection.Add(CreateCtrlButtonWithResult(mbox, No, false, null, defaultResult == MessageBoxResult.No));

                    mbox.CtrlButtonCollection.Add(CreateCtrlButtonWithResult(mbox, Cancel, null, null, defaultResult == MessageBoxResult.Cancel));
                    break;
                default:
                    mbox.CtrlButtonCollection.Add(CreateCtrlButtonWithResult(mbox, Ok, true, null, defaultResult == MessageBoxResult.OK));
                    break;
            }
            #endregion
            switch (icon)
            {
                case MessageBoxImage.Error:
                    mbox.UIcon = "\ue644";
                    mbox.UIconForeground = System.Windows.Media.Brushes.Red;
                    break;
                case MessageBoxImage.Information:
                    mbox.UIcon = "\ue659";
                    break;
                case MessageBoxImage.Question:
                    mbox.UIcon = "\ue60e";
                    break;
                case MessageBoxImage.Warning:
                    mbox.UIcon = "\ue60b";
                    break;
            }
            var result = mbox.ShowDialog();
            Ok = "Ok";
            Cancel = "Cancel";
            Yes = "Yes";
            No = "No";
            switch (button)
            {
                case MessageBoxButton.OKCancel:
                    {
                        return result == true ? MessageBoxResult.OK : result == false ? MessageBoxResult.Cancel : MessageBoxResult.None;
                    }
                case MessageBoxButton.YesNo:
                    {
                        return result == true ? MessageBoxResult.Yes : MessageBoxResult.No;
                    }
                case MessageBoxButton.YesNoCancel:
                    {
                        return result == true ? MessageBoxResult.Yes : result == false ? MessageBoxResult.No : MessageBoxResult.Cancel;
                    }
                case MessageBoxButton.OK:
                default:
                    {
                        return result == true ? MessageBoxResult.OK : MessageBoxResult.None;
                    }
            }
        }

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public ObservableCollection<UButton> CtrlButtonCollection
        {
            get { return (ObservableCollection<UButton>)GetValue(CtrlButtonCollectionProperty); }
            set { SetValue(CtrlButtonCollectionProperty, value); }
        }

        private static UButton CreateCtrlButton(string content)
        {
            UButton btn = new UButton();
            btn.Style = Application.Current.FindResource("MessageBoxUButton") as System.Windows.Style;
            btn.Content = content;
            btn.Focusable = true;
            return btn;
        }
        private static UButton CreateCtrlButtonWithResult(MessageBox mbox, string content, bool? dialogResult, Action action, bool isDefault = false)
        {
            var btn = CreateCtrlButton(content);
            btn.IsDefault = isDefault;
            btn.Click += (sender, args) =>
            {
                action?.Invoke();
                mbox.DialogResult = dialogResult;
                if (null == dialogResult)
                { mbox.Close(); }
            };
            return btn;
        }

        private static void Btn_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (null != CtrlButtonCollection)
            {
                CtrlButtonCollection.Clear();
                CtrlButtonCollection = new ObservableCollection<UButton>();
            }
            base.OnClosed(e);
        }

    }

}
