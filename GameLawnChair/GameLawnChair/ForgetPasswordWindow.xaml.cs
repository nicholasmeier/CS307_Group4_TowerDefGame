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
    /// Interaction logic for ForgetPasswordWindow.xaml
    /// </summary>
    public partial class ForgetPasswordWindow : Window
    {
        public const string FirebaseAppKey = "AIzaSyAiX70y-obo-vUyAO7-LJM8EwGcRD2UEWg";
        public const string FirebaseAppUrl = "https://cs-307-herotd.firebaseapp.com/";

        public ForgetPasswordWindow()
        {
            InitializeComponent();
        }

        private void reset_button_Click(object sender, RoutedEventArgs e)
        {
            var mainw = new MainWindow();
            mainw.Show();
            this.Close();
            SendPasswordResetEmailAsync(emailInput.Text);
        }

        private async void SendPasswordResetEmailAsync(string email)
        {
            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(FirebaseAppKey));
                await auth.SendPasswordResetEmailAsync(email);
            }
            catch (Exception e)
            {
                MessageBox.Show("Invalid Email");
            }
        }
    }
}
