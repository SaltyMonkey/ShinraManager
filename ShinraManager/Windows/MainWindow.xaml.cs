using ShinraManager.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ShinraManager.Autorun;
using ShinraManager.Settings;
namespace ShinraManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ClickThrouWindow
    {
        public MainWindow()
        {
            ManagerSettings.Instance.Refresh();
            InitializeComponent();
         
        }

        private void Logo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ProcessWorkWrapper.JustStartProcess("https://github.com/SaltyMonkey/ShinraManager");
        }

        private void HideWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Hide();
        }

        private void ShinraChoosePathB_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void TccChoosePathB_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ShinraClosePrB_Click(object sender, RoutedEventArgs e)
        {
            ProcessWorkWrapper.KillProcess(ManagerSettings.Instance.ShinraMeterDefaultName);
        }

        private void TccClosePrB_Click(object sender, RoutedEventArgs e)
        {
            ProcessWorkWrapper.KillProcess(ManagerSettings.Instance.TccDefaultName);
        }

    
    }
}
