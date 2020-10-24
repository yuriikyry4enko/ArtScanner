using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ArtScanner.Models.Entities;
using ArtScanner.Services;
using ArtScanner.Utils.Constants;
using ArtScanner.Utils.Helpers;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace ArtScanner.ViewModels
{
    class HomePageViewModel : BaseViewModel
    {
        private readonly IUserDialogs _userDialogs;
        private readonly IItemDBService _itemDBService;
        private readonly IAppFileSystemService _appFileSystemService;
        private readonly IAppDatabase _appDatabase;
        private readonly IAppSettings appSettings;


        private ObservableCollection<FolderItemEntity> _bookletItems = new ObservableCollection<FolderItemEntity>();
        public ObservableCollection<FolderItemEntity> BookletItems
        {
            get => _bookletItems;
            set => SetProperty(ref _bookletItems, value);
        }

        private FolderItemEntity _selectedBookletItem;
        public FolderItemEntity SelectedBookletItem
        {
            get => _selectedBookletItem;
            set => SetProperty(ref _selectedBookletItem, value);
        }


        private bool _isMessageAlertVisible = false;
        public bool IsMessageAlertVisible
        {
            get => _isMessageAlertVisible;
            set => SetProperty(ref _isMessageAlertVisible, value);
        }

        private bool _isFoldersListEmpty = true;
        public bool IsFoldersListEmpty
        {
            get => _isFoldersListEmpty;
            set => SetProperty(ref _isFoldersListEmpty, value);
        }


        #region Commands

        public ICommand ItemTappedCommad => new Command<FolderItemEntity>(async (item) =>
        {
            await navigationService.NavigateAsync(PageNames.BookletItemDetailsPage, CreateParameters(item));
        });

        public ICommand DeleteCommand => new Command(async (item) =>
        {
            var model = item as FolderItemEntity;

            var folderCategoriesCount = (await _itemDBService.GetCategoriesByParentIdAll(model.Id)).Count();
            var result = await _userDialogs.ConfirmAsync($"Remove folder with {folderCategoriesCount} categories?", "Confirmation", "Yes", "No");

            if (!result) return;

            await _itemDBService.DeleateFolderItem(model);

            BookletItems.Remove(model);

            if (BookletItems.Count == 0)
            {
                IsFoldersListEmpty = true;
            }
        });

        public ICommand NavigateToGalleyCommand => new Command(async () =>
        {
            await navigationService.NavigateAsync(PageNames.ItemsGalleryPage);
        });

        public ICommand CloseCommand => new Command(async () =>
        {
            await PopupNavigation.Instance.PopAsync();
        });


        public ICommand NavigateToBookletsCommand => new Command(async () => { await navigationService.NavigateAsync(PageNames.BookletPage); });

        public ICommand SettingsCommand => new Command(async () => { await navigationService.NavigateAsync(PageNames.ChooseLanguagePage); });

        public ICommand OpenBurgerMenuCommand => new Command(async () =>
        {
            await navigationService.NavigateAsync(PageNames.BurgerMenuPopupPage);
        });

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

                    if (status == PermissionStatus.Granted)
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


        #region Ctor

        public HomePageViewModel(
            INavigationService navigationService,
            IUserDialogs userDialogs,
            IAppFileSystemService appFileSystemService,
            IAppDatabase appDatabase,
            IAppSettings appSettings,
            IItemDBService itemDBService) : base(navigationService)
        {
            this._itemDBService = itemDBService;
            this._appFileSystemService = appFileSystemService;
            this._userDialogs = userDialogs;
            this._appDatabase = appDatabase;
            this.appSettings = appSettings;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            //if (appSettings.NeedToUpdateHomePage)
            //{
                await InitItemsList();
            //    appSettings.NeedToUpdateHomePage = false;
            //}

        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            this._appFileSystemService.InitializeFoldersForUser("sources");
            this._appDatabase.Initialize(_appFileSystemService.CurrentUserFolderPath);

        }

        #endregion

        private async Task InitItemsList()
        {
            try
            {
                BookletItems.Clear();

                var result = await _itemDBService.GetAllFolders();

                if (result.Count == 0)
                {
                    IsFoldersListEmpty = true;
                }
                else
                {
                    IsFoldersListEmpty = false;
                }

                foreach (var item in result)
                {
                    BookletItems.Add(new FolderItemEntity
                    {
                        LocalId = item.LocalId,
                        Id = item.Id,
                        Title = item.Title,
                        ImageByteArray = StreamHelpers.GetByteArrayFromFilePath(_appFileSystemService.GetFilePath(item.ImageFileName))
                    });
                }

            
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}