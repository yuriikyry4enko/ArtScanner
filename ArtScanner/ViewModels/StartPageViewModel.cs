using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using ArtScanner.Enums;
using ArtScanner.Services;
using ArtScanner.Utils.AuthConfigs;
using ArtScanner.Utils.Constants;
using ArtScanner.Views;
using LinqToTwitter;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Navigation;
using Xamarin.Forms;

namespace ArtScanner.ViewModels
{
    class StartPageViewModel : BaseViewModel
    {
        #region Ctor

        public StartPageViewModel(
            INavigationService navigationService) : base(navigationService)
        {
        }

        #endregion

        #region Commands

        public ICommand BackCommand => new Command(async () => { await navigationService.GoBackAsync(); });

        public ICommand ScannCommand => new Command(async () =>
        {
            try
            {
                if (OAuthConfig.User == null)
                {
                    OAuthConfig.navigationService = navigationService;
                    await navigationService.NavigateAsync(PageNames.ProviderLoginPage);
                }

                //var auth = new ApplicationOnlyAuthorizer()
                //{
                //    CredentialStore = new InMemoryCredentialStore
                //    {
                //        ConsumerKey = "9rD8HiNF8pvfGeZAeSL18DybE",
                //        ConsumerSecret = "PiwYPbE8P1TLGJjdbD8zeer0vMHydYw3qSDA6XC60cvmqt92tC",
                //        OAuthToken = "1041694396830240768-La7tKNMzP2c0rvBtmzp312NQcAgoQt",
                //        OAuthTokenSecret = "ZIz1keeE2WFl34sd34fsnSw7rQ6UXWbUiXxGWaMXHcVzF",
                //    },
                //};
                //await auth.AuthorizeAsync();

                //var ctx = new TwitterContext(auth);

                //await ctx.TweetAsync("Hello from LINQ to Twitter");
                //Search searchResponse = await
                //    (from search in ctx.Search
                //     where search.Type == SearchType.Search &&
                //           search.Query == "\"Twitter\""
                //     select search)
                //    .SingleAsync();

                //var Tweets =
                //    (from tweet in searchResponse.Statuses
                //     select new Tweet
                //     {
                //         StatusID = tweet.StatusID,
                //         ScreenName = tweet.User.ScreenNameResponse,
                //         Text = tweet.Text,
                //         ImageUrl = tweet.User.ProfileImageUrl
                //     })
                //    .ToList();
                //var status = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
                //if (status != PermissionStatus.Granted)
                //{
                //    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                //    {
                //        //await DisplayAlert("Need location", "Gunna need that location", "OK");
                //    }

                //    status = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                //}

                //if (status == PermissionStatus.Granted)
                //{
                //    await navigationService.NavigateAsync(PageNames.ScannerPage);
                //}
                //else if (status != PermissionStatus.Unknown)
                //{
                //    status = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                //}
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

           
        });

        #endregion
    }
}
