using System;
using System.Diagnostics;
using System.Windows.Input;
using ArtScanner.Services;
using ArtScanner.Utils.Constants;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Navigation;
using Xamarin.Forms;

namespace ArtScanner.ViewModels
{
    class StartPageViewModel : BaseViewModel
    {
        private IAppFileSystemService _appFileSystemService;
        private IAppDatabase _appDatabase;

        private bool _isMessageAlertVisible = false;
        public bool IsMessageAlertVisible
        {
            get => _isMessageAlertVisible;
            set => SetProperty(ref _isMessageAlertVisible, value);
        }

        #region Ctor

        public StartPageViewModel(
            IAppDatabase appDatabase,
            IAppFileSystemService appFileSystemService,
            INavigationService navigationService) : base(navigationService)
        {
            this._appFileSystemService = appFileSystemService;
            this._appDatabase = appDatabase;
        }


        #endregion

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            this._appFileSystemService.InitializeFoldersForUser("sources");
            this._appDatabase.Initialize(_appFileSystemService.CurrentUserFolderPath);
        }

        #region Commands

        public ICommand NavigateToGalleyCommand => new Command(async () =>
        {
            await navigationService.NavigateAsync(PageNames.ItemsGalleryPage);
        });

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
                        //await DisplayAlert("Need permission", "", "OK");
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();

                    if(status == PermissionStatus.Granted)
                    {
                        IsMessageAlertVisible = true;
                    }

                    return;
                }

                if (status == PermissionStatus.Granted)
                {
                    IsMessageAlertVisible = false;
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
