using System;
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
                });
            }
        }

    }
}
