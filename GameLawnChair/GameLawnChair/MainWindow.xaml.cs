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
using Firebase.Auth;

namespace GameLawnChair
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string FirebaseAppKey = "AIzaSyAiX70y-obo-vUyAO7-LJM8EwGcRD2UEWg";
        public const string FirebaseAppUrl = "https://cs-307-herotd.firebaseapp.com/";


        public MainWindow()
        {
            InitializeComponent();
        }

        
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameinput.Text;
            string password = PasswordInput.Password;
            if (password.Length == 0) //TODO add more error conditions
            {
                MessageBox.Show("Invalid Password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (username.Length == 0)
            {
                MessageBox.Show("Invalid Username", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            FetchLogin(username, password);
            

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

        private async void FetchLogin(string email, string pass)
        {
            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseAppKey));
                var data = await auth.SignInWithEmailAndPasswordAsync(email, pass);
            }
            catch (Exception e)
            {
                MessageBox.Show("Invalid Username or Password");
                //MessageBox.Show(e.ToString());

            }
        }


    }
}
