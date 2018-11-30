using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util;
using Firebase;
using Firebase.Auth;


namespace GameLawnChair
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public const string FirebaseAppKey = "AIzaSyAiX70y-obo-vUyAO7-LJM8EwGcRD2UEWg";
        public const string FirebaseAppUrl = "https://cs-307-herotd.firebaseapp.com/";
        public const string GoogleID = "326999430623-3sqk83o24q6rdfhods04vkmrc72e3api.apps.googleusercontent.com";

        public Window1()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //launch into google
            if (textforaccount.Text.Length == 0)
            {
                msgforacc.Text = "type email here";
            }
            if (textforpw.Password.Length == 0)
            {
                msgforpswd.Text = "type password here";
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var win = new MainWindow();
            win.Show();
            this.Close();
        }

        private async void GoogleClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var cr = new PromptCodeReceiver();

                var result = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets { ClientId = GoogleID },
                    new[] { "email", "profile" },
                    "user",
                    CancellationToken.None);

                if (result.Token.IsExpired(SystemClock.Default))
                {
                    await result.RefreshTokenAsync(CancellationToken.None);
                }

                //this.FetchFirebaseData(result.Token.AccessToken, FirebaseAuthType.Google);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
