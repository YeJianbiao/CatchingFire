using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;

namespace UtilityControl.Control
{
    public class PopupExtession : Popup
    {

        public static DependencyProperty TopmostProperty = Window.TopmostProperty.AddOwner(typeof(PopupExtession), new FrameworkPropertyMetadata(false, OnTopmostChanged));

        public bool Topmost
        {
            get { return (bool)GetValue(TopmostProperty); }
            set { SetValue(TopmostProperty, value); }
        }

        private static void OnTopmostChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            (obj as PopupExtession)?.UpdateWindow();
        }
        protected override void OnOpened(EventArgs e)
        {
            UpdateWindow();
        }
        private void UpdateWindow()
        {
            var hwndSource = (HwndSource)PresentationSource.FromVisual(this);
            if (hwndSource != null)
            {
                var hwnd = hwndSource.Handle;
                RECT rect;
                if (GetWindowRect(hwnd, out rect))
                {
                    SetWindowPos(hwnd, Topmost ? -1 : -2, rect.Left, rect.Top, double.IsNaN(Width) ? (int)((Window)hwndSource.RootVisual).ActualWidth : (int)(Width), double.IsNaN(Height) ? (int)((Window)hwndSource.RootVisual).ActualHeight : (int)(Height), 0);
                }
            }
        }
        #region imports definitions
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32", EntryPoint = "SetWindowPos")]
        private static extern int SetWindowPos(IntPtr hWnd, int hwndInsertAfter, int x, int y, int cx, int cy, int wFlags);
        #endregion
    }
}
