using Android.App;
using Xamarin.Forms.Platform.Android;
using ArtScanner.Views;
using Xamarin.Forms;
using ArtScanner.Droid.Renderers;
using Android.Content;
using ArtScanner.Utils.AuthConfigs;

[assembly: ExportRenderer(typeof(ProviderLoginPage), typeof(ProviderLoginPageRenderer))]
namespace ArtScanner.Droid.Renderers
{
    public class ProviderLoginPageRenderer : PageRenderer
    {
        public ProviderLoginPageRenderer(Context context) : base(context)
        {
           
        }

        bool showLogin = true;
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            //Get and Assign ProviderName from ProviderLoginPage  
            var loginPage = Element as ProviderLoginPage;
            var activity = this.Context as Activity;
            if (showLogin && OAuthConfig.User == null)
            {
                showLogin = false;
                //Create OauthProviderSetting class with Oauth Implementation .Refer Step 6  
                OAuthProviderSetting oauth = new OAuthProviderSetting();
                var auth = oauth.LoginWithTwitter();
                // After facebook,google and all identity provider login completed   
                auth.Completed += (sender, eventArgs) => {
                    if (eventArgs.IsAuthenticated)
                    {
                        OAuthConfig.User = new UserDetails();
                        // Get and Save User Details   
                        OAuthConfig.User.Token = eventArgs.Account.Properties["oauth_token"];
                        OAuthConfig.User.TokenSecret = eventArgs.Account.Properties["oauth_token_secret"];
                        OAuthConfig.User.TwitterId = eventArgs.Account.Properties["user_id"];
                        OAuthConfig.User.ScreenName = eventArgs.Account.Properties["screen_name"];
                       
                    }

                    OAuthConfig.SuccessfulLoginAction.Invoke();
                };
                activity.StartActivity(auth.GetUI(activity));
            }
        }
    }
}