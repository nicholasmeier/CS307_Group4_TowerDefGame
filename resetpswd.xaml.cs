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
    /// Interaction logic for resetpswd.xaml
    /// </summary>
    public partial class resetpswd : Window
    {
        public resetpswd()
        {
            InitializeComponent();
        }

        private void sendrecoveryemail_Click(object sender, RoutedEventArgs e)
        {
            //send an email out
            var mainw = new MainWindow();
            mainw.Show();
            this.Close();
        }

    }
}
