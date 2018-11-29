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

namespace GameLawnChair
{
    /// <summary>
    /// fblogin.xaml 的交互逻辑
    /// </summary>
    public partial class fblogin : Window
    {
        public fblogin()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //launch the game
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //go back to main
            var win = new MainWindow();
            win.Show();
            this.Close();
        }
    }
}
