using System;
using System.Diagnostics;
using ArtScanner.Utils.Constants;
using LinqToTwitter;
using Prism.Navigation;
using Xamarin.Forms;

namespace ArtScanner.Utils.AuthConfigs
{
   public class OAuthConfig
   {
        public static INavigationService navigationService;

        public static UserDetails User;

        public static Action SuccessfulLoginAction
        {
            get
            {
                return new Action(async () =>
                {
                    await navigationService.GoBackAsync();

                    if (User != null)
                    {
                        try
                        {
                            var auth = new SingleUserAuthorizer()
                            {
                                CredentialStore = new SingleUserInMemoryCredentialStore
                                {
                                    ConsumerKey = ApiConstants.consumerKey,
                                    ConsumerSecret = ApiConstants.consumerSecret,
                                    OAuthToken = OAuthConfig.User.Token,
                                    OAuthTokenSecret = OAuthConfig.User.TokenSecret,

                                }
                            };

                            var context = new TwitterContext(auth);

                            await context.TweetAsync(
                                  "Hello World!"
                              );
                        }
                        catch(Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }
                    }
                });
            }
        }

    }
}
