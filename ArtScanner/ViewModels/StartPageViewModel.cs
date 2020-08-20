using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using ArtScanner.Services;
using ArtScanner.Utils.Constants;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.SimpleAudioPlayer;
using Prism.Navigation;
using Xamarin.Forms;

namespace ArtScanner.ViewModels
{
    class StartPageViewModel : BaseViewModel
    {
        private IAppFileSystemService _appFileSystemService;
        private IAppDatabase _appDatabase;
        private ISettings _settings;

        #region Ctor

        public StartPageViewModel(
            ISettings settings,
            IAppDatabase appDatabase,
            IAppFileSystemService appFileSystemService,
            INavigationService navigationService) : base(navigationService)
        {
            this._appFileSystemService = appFileSystemService;
            this._settings = settings;
            this._appDatabase = appDatabase;
        }


        #endregion

        #region Command

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (!_settings.IsUserFolderInitialized)
            {
                this._appFileSystemService.InitializeFoldersForUser("sources");
                this._appDatabase.Initialize(_appFileSystemService.CurrentUserFolderPath);

                _settings.IsUserFolderInitialized = true;
            }
        }

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
                        //await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                    return;
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
