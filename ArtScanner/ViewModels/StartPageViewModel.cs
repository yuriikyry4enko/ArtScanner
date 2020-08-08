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
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                    {
                        //await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                }

                if (status == PermissionStatus.Granted)
                {
                    await navigationService.NavigateAsync(PageNames.ScannerPage);
                }
                else if (status != PermissionStatus.Unknown)
                {
                    status = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

           
        });

        #endregion
    }
}
