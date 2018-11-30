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
using System.Windows.Shapes;
using System.Diagnostics;

namespace GameLawnChair
{
    /// <summary>
    /// personalpage.xaml 的交互逻辑
    /// </summary>
    public partial class personalpage : Window
    {
        public personalpage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //start game
            Process.Start("HeroTD.exe");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //logout to main window.
            var win = new MainWindow();
            win.Show();
            this.Close();
        }
    }
}
