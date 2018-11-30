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
using System.Dynamic;
using Firebase;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Auth;
using Facebook;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util;


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
            var FB_URI = GenerateFbLoginURI(FbAppID, "");
            this.Browser.Visibility = Visibility.Visible;
            this.Browser.Navigate(FB_URI);
            //if login failed...
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var FB_URI = GenerateFbLogoutURI(FbAppID, "");
            var asdf = 0;
            this.Browser.Visibility = Visibility.Visible;
            string cookie = String.Format("c_user=; expires={0:R}; path=/; domain=.facebook.com", DateTime.UtcNow.AddDays(-1).ToString("R"));
            Application.SetCookie(new Uri("https://www.facebook.com"), cookie);
            //go back to main
            /*var win = new MainWindow();
            win.Show();
            this.Close();*/
        }

        //The following method has used from https://www.hackviking.com/development/facebook-api-login-flow-for-desktop-application/
        private Uri GenerateFbLoginURI(string appID, string ext_perm)
        {
            dynamic parameters = new ExpandoObject();
            parameters.client_id = appID;
            parameters.redirect_uri = "https://www.facebook.com/connect/login_success.html";

            // The requested response: an access token (token), an authorization code (code), or both (code token).
            //parameters.response_type = "token";

            // list of additional display modes can be found at http://developers.facebook.com/docs/reference/dialogs/#display
            //parameters.display = "popup";

            // add the 'scope' parameter only if we have extendedPermissions.
            if (!string.IsNullOrWhiteSpace(ext_perm))
                parameters.scope = ext_perm;

            // generate the login url
            var fb = new FacebookClient();
            return fb.GetLoginUrl(parameters);
        }

        private Uri GenerateFbLogoutURI(string appID, string ext_perm)
        {
            dynamic parameters = new ExpandoObject();
            parameters.client_id = appID;
            parameters.redirect_uri = "https://www.facebook.com/connect/login_success.html";

            // The requested response: an access token (token), an authorization code (code), or both (code token).
            parameters.response_type = "token";

            // list of additional display modes can be found at http://developers.facebook.com/docs/reference/dialogs/#display
            parameters.display = "popup";

            // add the 'scope' parameter only if we have extendedPermissions.
            if (!string.IsNullOrWhiteSpace(ext_perm))
                parameters.scope = ext_perm;

            // generate the login url
            var fb = new FacebookClient();
            return fb.GetLogoutUrl(parameters);
        }

        private void BrowserNavigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            var fb = new FacebookClient();
            FacebookOAuthResult oauthResult;
            if (!fb.TryParseOAuthCallbackUrl(e.Uri, out oauthResult))
            {
                return;
            }

            if (oauthResult.IsSuccess)
            {
                //this.Browser.Visibility = Visibility.Collapsed;
                MessageBox.Show("It works");
                //this.FetchFirebaseData(oauthResult.AccessToken, FirebaseAuthType.Facebook);

            }
        }
    }
}
