using ShinraManager.Helpers.Native;
using ShinraManager.View;
using System;
using System.Threading;
using System.Windows;
using NLog;

namespace ShinraManager
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex _mutex = null;
        private string _appNameForActivate = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        private bool _newInst;
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SingleInstanceRunCheck();

            MainWindow wnd = new MainWindow();
            if (e.Args.Length > 0)
            {
                if (e.Args[0] == "-minimized")
                {
                    wnd.Visibility = Visibility.Hidden;
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
            if (!_newInst)
            {
                ActivateFirstInst();
                Current.Shutdown();
            }
        }

        private void ActivateFirstInst()
        {
            try
            {
                var ptr = UnsafeAPI.FindWindow(null, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
                if (ptr != IntPtr.Zero)
                {
                    UnsafeAPI.SetForegroundWindow(ptr);
                    if (UnsafeAPI.IsIconic(ptr))
                        UnsafeAPI.OpenIcon(ptr);
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex, "ActivateFirstInst");
            }
        }
    }
}