using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using System;
using System.Threading.Tasks;

namespace Player.Views.CustomControls
{
    public class CustomTitleBar : TemplatedControl
    {
        Button? _closeButton;
        Button? _minimizeButton;
        Button? _maximizeButton;
        //Border? _titleBar;
        Path? _maximizeIcon;
        ToolTip? _maximizeToolTip;

        //bool _mouseDownForWindowMoving = false;
        //PointerPoint _originalPoint;

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);      

            // if we had a control template before, we need to unsubscribe any event listeners
            if (_closeButton is not null)
            {
                _closeButton.Click -= _closeButton_Click;
            }

            if (_minimizeButton is not null)
            {
                _minimizeButton.Click -= _minimizeButton_Click;
            }

            if (_maximizeButton is not null)
            {
                _maximizeButton.Click -= _maximizeButton_Click;
            }

            //if(_titleBar is not null)
            //{
            //    _titleBar.PointerPressed -= _titleBar_PointerPressed;
            //    _titleBar.PointerMoved -= _titleBar_PointerMoved;
            //    _titleBar.PointerReleased -= _titleBar_PointerReleased;
            //}

            // try to find the control with the given name
            _closeButton = e.NameScope.Find("CloseButton") as Button;
            _minimizeButton = e.NameScope.Find("MinimizeButton") as Button;
            _maximizeButton = e.NameScope.Find("MaximizeButton") as Button;
            //_titleBar = e.NameScope.Find("TitleBar") as Border;
            _maximizeIcon = e.NameScope.Find("MaximizeIcon") as Path;
            _maximizeToolTip = e.NameScope.Find("MaximizeToolTip") as ToolTip;

            // listen to pointer-released events on the stars presenter.
            if (_closeButton is not null)
            {
                _closeButton.Click += _closeButton_Click;
            }

            if (_minimizeButton is not null)
            {
                _minimizeButton.Click += _minimizeButton_Click;
            }

            if (_maximizeButton is not null)
            {
                _maximizeButton.Click += _maximizeButton_Click;
            }

            //if (_titleBar is not null)
            //{
            //    _titleBar.PointerPressed += _titleBar_PointerPressed;
            //    _titleBar.PointerMoved += _titleBar_PointerMoved;
            //    _titleBar.PointerReleased += _titleBar_PointerReleased;
            //}

            SubscribeToWindowState();
        }

        private async void SubscribeToWindowState()
        {
            Window hostWindow = (Window)this.VisualRoot;

            while (hostWindow == null)
            {
                hostWindow = (Window)this.VisualRoot;
                await Task.Delay(50);
            }

            hostWindow.GetObservable(Window.WindowStateProperty).Subscribe(s =>
            {
                if (s != WindowState.Maximized)
                {
                    _maximizeIcon.Data = Geometry.Parse("M2048 2048v-2048h-2048v2048h2048zM1843 1843h-1638v-1638h1638v1638z");
                    hostWindow.Padding = new Thickness(0, 0, 0, 0);
                    _maximizeToolTip.Content = "Maximize";
                }
                if (s == WindowState.Maximized)
                {
                    _maximizeIcon.Data = Geometry.Parse("M2048 1638h-410v410h-1638v-1638h410v-410h1638v1638zm-614-1024h-1229v1229h1229v-1229zm409-409h-1229v205h1024v1024h205v-1229z");
                    hostWindow.Padding = new Thickness(7, 7, 7, 7);
                    _maximizeToolTip.Content = "Restore Down";

                    // This should be a more universal approach in both cases, but I found it to be less reliable, when for example double-clicking the title bar.
                    /*hostWindow.Padding = new Thickness(
                            hostWindow.OffScreenMargin.Left,
                            hostWindow.OffScreenMargin.Top,
                            hostWindow.OffScreenMargin.Right,
                            hostWindow.OffScreenMargin.Bottom);*/
                }
            });
        }

        private void _maximizeButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Window? hostWindow = this.VisualRoot as Window;

            if (hostWindow is not null)
            {
                hostWindow.WindowState = hostWindow.WindowState == WindowState.Normal ?
                                         WindowState.Maximized :
                                         WindowState.Normal;
            }
        }

        private void _minimizeButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Window? hostWindow = this.VisualRoot as Window;
            if (hostWindow is not null)
            {
                hostWindow.WindowState = WindowState.Minimized;
            }
        }

        private void _closeButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Window? hostWindow = this.VisualRoot as Window;
            hostWindow?.Close();
        }

        //private void _titleBar_PointerMoved(object? sender, PointerEventArgs e)
        //{
        //    if (!_mouseDownForWindowMoving) 
        //        return;

        //    Window? hostWindow = this.VisualRoot as Window;
        //    if (hostWindow is not null)
        //    {
        //        PointerPoint currentPoint = e.GetCurrentPoint(this);
        //        hostWindow.Position = new PixelPoint(hostWindow.Position.X + (int)(currentPoint.Position.X - _originalPoint.Position.X),
        //        hostWindow.Position.Y + (int)(currentPoint.Position.Y - _originalPoint.Position.Y));
        //    }

        //}

        //private void _titleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
        //{
        //    Window? hostWindow = this.VisualRoot as Window;
        //    if (hostWindow?.WindowState == WindowState.Maximized || hostWindow?.WindowState == WindowState.FullScreen)
        //    {
        //        double maxWidth = hostWindow.Width;
        //        hostWindow.WindowState = WindowState.Normal;
        //        double normalWidth = hostWindow.Width;
        //        PointerPoint currentPoint = e.GetCurrentPoint(this);
        //        //hostWindow.Position = new PixelPoint(hostWindow.Position.X + (int)(maxWidth - normalWidth), hostWindow.Position.Y);
        //        //hostWindow.Position = new PixelPoint(hostWindow.Position.X + (int)(currentPoint.Position.X - _originalPoint.Position.X),
        //        //hostWindow.Position.Y + (int)(currentPoint.Position.Y - _originalPoint.Position.Y));
        //    }

        //    _mouseDownForWindowMoving = true;
        //    _originalPoint = e.GetCurrentPoint(this);
        //}

        //private void _titleBar_PointerReleased(object? sender, PointerReleasedEventArgs e)
        //{
        //    _mouseDownForWindowMoving = false;
        //}
    }
}
