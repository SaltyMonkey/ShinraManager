using ShinraManager.Helpers;
using ShinraManager.Helpers.UI;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using System.Windows.Threading;
namespace ShinraManager.UI
{
    public class ClickThrouWindow : Window, INotifyPropertyChanged
    {
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_NOACTIVATE = 0x08000000;

        private double _scale = 1;

        private readonly Dispatcher _dispatcher;
        public event PropertyChangedEventHandler PropertyChanged;

        public double Scale
        {
            get => _scale;
            set
            {
                if (value == _scale) return;
                _scale = value;
                _dispatcher.InvokeIfRequired(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Scale")), DispatcherPriority.DataBind);
            }
        }

        private bool _snappedToBottom;
        public bool SnappedToBottom
        {
            get => _snappedToBottom;
            set
            {
                if (value == _snappedToBottom) return;
                _snappedToBottom = value;
                _dispatcher.InvokeIfRequired(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SnappedToBottom")), DispatcherPriority.DataBind);
            }
        }

        public bool DontClose = false;
        public ClickThrouWindow()
        {
            SnapsToDevicePixels = true;
            AllowsTransparency = true;
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight - 200;
            Closing += ClickThrouWindow_Closing;
            WindowStyle = WindowStyle.None;
            Focusable = true;
            BorderThickness = new Thickness(0);
            Topmost = true;
            ShowInTaskbar = true;

            SizeToContent = SizeToContent.WidthAndHeight;

            MouseLeftButtonDown += Move;
            Loaded += (s, a) =>
            {
                MinWidth = MinWidth * Scale;
                MinHeight = MinHeight * Scale;

            };
            ShowActivated = false;
            WindowStartupLocation = WindowStartupLocation.Manual;
            ResizeMode = ResizeMode.NoResize;
            _dispatcher = Dispatcher.CurrentDispatcher;
            _opacity = 0.7;

        }

        public Point? LastSnappedPoint = null;
        internal bool _dragged = false;
        private bool _dragging = false;
        private int _oldHeight;
        private int _oldWidth;
        private Thickness _margin;
        private double _opacity;
        public bool Visible;
        protected virtual bool Empty => false;



        public void SetClickThrou()
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            WindowsServices.SetWindowExTransparent(hwnd);
        }

        public void UnsetClickThrou()
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            WindowsServices.SetWindowExVisible(hwnd);
        }

        public void Move(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.LeftButton != MouseButtonState.Pressed) return;
                _dragging = true;
                DragMove();
                _dragged = true;
                _dragging = false;

            }
            catch { Console.WriteLine(@"Exception Move"); }
        }

        protected void ClickThrouWindow_Closing(object sender, CancelEventArgs e)
        {
            if (DontClose)
            {
                e.Cancel = true;
                if (Visibility == Visibility.Visible) HideWindow();
                return;
            }
            Closing -= ClickThrouWindow_Closing;
            foreach (ClickThrouWindow window in ((ClickThrouWindow)sender).OwnedWindows)
            {
                window.DontClose = false;
                window.Close();
            }

            _dispatcher.BeginInvoke(new Action(() =>
            {
                var a = OpacityAnimation(0);
                a.Completed += (o, args) => { Close(); };
                BeginAnimation(OpacityProperty, a);
            }));
            e.Cancel = true;

        }

        public void HideWindow(bool set = false)
        {
            Visible = set;

            _dispatcher.BeginInvoke(new Action(() =>
            {
                var a = OpacityAnimation(0);
                a.Completed += (o, args) => { Visibility = Visibility.Hidden; };
                BeginAnimation(OpacityProperty, a);
            }));

        }

        public void ShowWindow()
        {
            Visible = true;
            if (Empty) return;
            _dispatcher.BeginInvoke(new Action(() =>
            {
                BeginAnimation(OpacityProperty, OpacityAnimation(_opacity));
            }));

            Visibility = Visibility.Visible;
            //Opacity = 0;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            source.AddHook(WindowsServices.ClickNoFocus);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            //Set the window style to noactivate.
            var helper = new WindowInteropHelper(this);
            UnsafeAPI.SetWindowLong(helper.Handle, GWL_EXSTYLE, UnsafeAPI.GetWindowLong(helper.Handle, GWL_EXSTYLE) | WS_EX_NOACTIVATE);
        }

        private static DoubleAnimation OpacityAnimation(double to)
        {
            return new DoubleAnimation(to, TimeSpan.FromMilliseconds(300)) { EasingFunction = new QuadraticEase() };
        }


    }
   
}
