using System;
using System.Diagnostics;
using System.Runtime.Remoting.Contexts;
using ArtScanner.iOS.Renderers;
using ArtScanner.Utils.AuthConfigs;
using ArtScanner.Views;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ProviderLoginPage), typeof(ProviderLoginPageRenderer))]
namespace ArtScanner.iOS.Renderers
{
    public class ProviderLoginPageRenderer : PageRenderer
    {
        bool showLogin = true;
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            try
            {
                var loginPage = Element as ProviderLoginPage;
                if (showLogin && OAuthConfig.User == null)
                {
                    var window = UIApplication.SharedApplication.KeyWindow;
                    var vc = window.RootViewController;

                    showLogin = false;
                    OAuthProviderSetting oauth = new OAuthProviderSetting();
                    var auth = oauth.LoginWithTwitter();
                    
                    auth.Completed += (sender, eventArgs) =>
                    {
                        if (eventArgs.IsAuthenticated)
                        {
                            vc.DismissViewController(true, null);
                            OAuthConfig.User = new UserDetails();
                            // Get and Save User Details   
                            OAuthConfig.User.Token = eventArgs.Account.Properties["oauth_token"];
                            OAuthConfig.User.TokenSecret = eventArgs.Account.Properties["oauth_token_secret"];
                            OAuthConfig.User.TwitterId = eventArgs.Account.Properties["user_id"];
                            OAuthConfig.User.ScreenName = eventArgs.Account.Properties["screen_name"];

                        }

                        OAuthConfig.SuccessfulLoginAction.Invoke();
                    };

                    var ui = auth.GetUI();
                    while (vc.PresentedViewController != null)
                    {
                        vc = vc.PresentedViewController;
                    }
                    vc.PresentViewController(ui, true, null);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}