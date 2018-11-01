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
    /// Interaction logic for SignupWindow.xaml
    /// </summary>
    public partial class SignupWindow : Window
    {
        public SignupWindow()
        {
            InitializeComponent();
        }

        private void confirmbut_Click(object sender, RoutedEventArgs e)
        {
            //do some checking, return to the mainwindow if all checking passed
            // for now it just closes and goes to the main window.
            var mainw = new MainWindow();
            mainw.Show();
            this.Close();
        }
    }
}
