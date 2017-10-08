using ShinraManager.Helpers;
using System;
using System.Threading;
using System.Windows;

namespace ShinraManager
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App
    {
        private static Mutex _mutex = null;
        private readonly string _appNameForActivate = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        private bool _newInst;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SingleInstanceRunCheck();

            var wnd = new MainWindow();
            if (e.Args.Length > 0)
            {
                if (e.Args[0] == "-minimized")
                {
                    wnd.Visibility = Visibility.Hidden;
                    wnd.ShowTaskbarIcon();
                }
                else
                {
                    wnd.Show();
                }
            }
            else
            {
                wnd.Show();
            }
        }

        private void SingleInstanceRunCheck()
        {
            _mutex = new Mutex(true, _appNameForActivate, out _newInst);
            if (_newInst) return;
            ActivateFirstInst();
            Current.Shutdown();
        }

        private void ActivateFirstInst()
        {
            try
            {
                var ptr = UnsafeAPI.FindWindow(null, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
                if (ptr == IntPtr.Zero) return;
                UnsafeAPI.SetForegroundWindow(ptr);
                if (UnsafeAPI.IsIconic(ptr))
                    UnsafeAPI.OpenIcon(ptr);
            }
            catch (Exception ex)
            {
                // ignored
            }
        }
    }
}
