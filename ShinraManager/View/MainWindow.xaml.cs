using ShinraManager.Helpers;
using System.Windows;

namespace ShinraManager.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            IClosingHack context = DataContext as IClosingHack;
            if (context != null)
            {
                e.Cancel = !context.OnClosing();
            }
        }
    }
}