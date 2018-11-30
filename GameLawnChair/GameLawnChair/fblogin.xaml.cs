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
using Firebase;
using Facebook;

namespace GameLawnChair
{
    /// <summary>
    /// fblogin.xaml 的交互逻辑
    /// </summary>
    public partial class fblogin : Window
    {
        public const string FbAppID = "308275636447469";
        public const string FirebaseAppKey = "AIzaSyAiX70y-obo-vUyAO7-LJM8EwGcRD2UEWg";
        public const string FirebaseAppUrl = "https://cs-307-herotd.firebaseapp.com/";

        public fblogin()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //launch the game
            if (textforaccount.Text.Length == 0) {
                msgforacc.Text = "type email here";
            }
            if (textforpw.Password.Length == 0) {
                msgforpswd.Text = "type password here";
            }
            //if login failed...

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
