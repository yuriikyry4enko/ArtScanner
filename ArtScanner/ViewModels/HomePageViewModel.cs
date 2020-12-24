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
using ArtScanner.Views;
using MediaManager.Media;
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
        private readonly IAppSettings _appSettings;

        public static bool NeedsToUpdate = true;

        private ObservableCollection<ItemEntity> _bookletItems = new ObservableCollection<ItemEntity>();
        public ObservableCollection<ItemEntity> BookletItems
        {
            get => _bookletItems;
            set => SetProperty(ref _bookletItems, value);
        }

        private ItemEntity _selectedBookletItem;
        public ItemEntity SelectedBookletItem
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

        private int _opacityButton = 0;
        public int OpacityButton 
        {
            get => _opacityButton;
            set => SetProperty(ref _opacityButton, value);
        }


        #region Commands

        public ICommand ItemTappedCommad => new Command<ItemEntity>(async (item) =>
        {
            await navigationService.NavigateAsync(PageNames.BookletItemDetailsPage, CreateParameters(item));
        });

        public ICommand DeleteCommand => new Command(async (item) =>
        {
            var model = item as ItemEntity;

            var folderCategoriesCount = (await _itemDBService.GetItemsByParentIdAll(model.Id)).Count();
            var result = await _userDialogs.ConfirmAsync($"Remove {folderCategoriesCount} items?", "Confirmation", "Yes", "No");

            if (!result) return;

            await _itemDBService.DeleateItemWithChildren(model);

            BookletItems.Remove(model);

            if (BookletItems.Count == 0)
            {
                IsFoldersListEmpty = true;
                OpacityButton = 1;
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

        public ICommand CodeTypingCommand => new Command(async () =>
        {
            if(_appSettings.ActiveFolderId == 0)
            {
                await _userDialogs.AlertAsync("you need to activate some folder", "No active folders", "Ok");
                return;
            }

            await navigationService.NavigateAsync(PageNames.CodeTypingPage);
        });

        public ICommand SettingsCommand => new Command(async () => { await navigationService.NavigateAsync(PageNames.ChooseLanguagePage); });
        
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
                LogService.Log(ex);
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
            this._appSettings = appSettings;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (BookletItems.Count() == 0)
            {
                await InitItemsList();
            }

        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            this._appFileSystemService.InitializeFoldersForUser("sources");
            this._appDatabase.Initialize(_appFileSystemService.CurrentUserFolderPath);

            _appSettings.NeedToUpdateHomePage = true;
        }

        #endregion

        private async Task InitItemsList()
        {
            try
            {
                var result = await _itemDBService.GetAllMainFolders();

                if (result.Count == 0)
                {
                    IsFoldersListEmpty = true;
                    OpacityButton = 1;
                }
                else
                {
                    IsFoldersListEmpty = false;
                    OpacityButton = 0;
                }

                if (_appSettings.NeedToUpdateHomePage && BookletItems.Count() == 0)
                {
                    BookletItems.Clear();

                    foreach (var item in result)
                    {
                        BookletItems.Add(new ItemEntity
                        {
                            LocalId = item.LocalId,
                            Id = item.Id,
                            Title = item.Title,
                            IsFolder = item.IsFolder,
                            ParentId = item.ParentId,
                            ImageByteArray = StreamHelpers.GetByteArrayFromFilePath(_appFileSystemService.GetFilePath(item.ImageFileName, FileType.Image))
                        });
                    }

                    _appSettings.NeedToUpdateHomePage = false;
                }
            }
            catch (Exception ex)
            {
                LogService.Log(ex);
            }
        }
    }
}