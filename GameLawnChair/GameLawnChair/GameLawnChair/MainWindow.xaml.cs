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

namespace GameLawnChair
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameinput.Text;
            string password = PasswordInput.Password;
            /*outputarea.Inlines.Add(username);
            if (password != "") {
                outputarea.Inlines.Add(password);
            }*/

        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            var win = new SignupWindow();
            win.Show();
            this.Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void passwordreset_Click(object sender, RoutedEventArgs e)
        {
            var win = new resetpswd();
            win.Show();
            this.Close();
        }

        private void passwordmanuallyreset_Click(object sender, RoutedEventArgs e)
        {
            var win = new ForgetPasswordWindow();
            win.Show();
            this.Close();
        }
    }
}
