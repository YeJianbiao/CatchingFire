
using System.Windows;
using System.Windows.Input;
using Utility.Sys;

namespace UtilityControl.Control
{
    class TopmostWindow : Window
    {

        public TopmostWindow()
        {
            Topmost = true;
            WindowState = WindowState.Normal;
            AllowsTransparency = true;
            ShowActivated = false;
            ShowInTaskbar = false;

            QueryContinueDrag += DragSource_QueryContinueDrag;

            Closed += (sender, args) =>
            {
                QueryContinueDrag -= DragSource_QueryContinueDrag;
            };
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DataObject data = new DataObject(typeof(object), this);
                DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
            }
            base.OnMouseMove(e);
        }

        void DragSource_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            UpdateWindowLocation();
        }

        private void UpdateWindowLocation()
        {
            Win32.Point p;
            if (!Win32.GetCursorPos(out p))
            {
                return;
            }
            Left = p.X;
            Top = p.Y;
        }
    }
}
