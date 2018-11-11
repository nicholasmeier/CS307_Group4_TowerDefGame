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
using Firebase.Auth;

namespace GameLawnChair
{
    /// <summary>
    /// Interaction logic for SignupWindow.xaml
    /// </summary>

    public partial class SignupWindow : Window
    {
        public const string FirebaseAppKey = "AIzaSyAiX70y-obo-vUyAO7-LJM8EwGcRD2UEWg";
        public const string FirebaseAppUrl = "https://cs-307-herotd.firebaseapp.com/";

        public SignupWindow()
        {
            InitializeComponent();
        }

        private void confirmbut_Click(object sender, RoutedEventArgs e)
        {
            //do some checking, return to the mainwindow if all checking passed
            // for now it just closes and goes to the main window.
            string email = emailinput.Text;
            string user = usernameinput.Text;
            string pass = passwordinput.Text;

            if ((email.Length == 0) || (pass.Length == 0))
            {

            }
            firebaseSignin(email, pass, user);
            var mainw = new MainWindow();
            mainw.Show();
            this.Close();
        }

        private async void firebaseSignin(string email, string password, string username)
        {
            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseAppKey));
                var data = await auth.CreateUserWithEmailAndPasswordAsync(email, password, username);
                var linkedAccounts = await auth.GetLinkedAccountsAsync(email);
                if (linkedAccounts.IsRegistered == true)
                {
                    MessageBox.Show("Success");
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Error");
            }
        }
    }
}
