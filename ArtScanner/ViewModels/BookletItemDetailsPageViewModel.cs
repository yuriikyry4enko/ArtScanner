using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ArtScanner.Models;
using ArtScanner.Models.Entities;
using ArtScanner.Resources;
using ArtScanner.Resx;
using ArtScanner.Services;
using ArtScanner.Utils.Constants;
using ArtScanner.Utils.Helpers;
using ArtScanner.Views;
using Prism.Navigation;
using Xamarin.Forms;

namespace ArtScanner.ViewModels
{
    class BookletItemDetailsPageViewModel : BaseViewModel
    {
        private readonly IUserDialogs _userDialogs;
        private readonly IItemDBService _itemDBService;
        private readonly IAppFileSystemService _appFileSystemService;
        private readonly IAppSettings appSettings;
        private readonly IRestService _restService;

        private long _ParentItemEntityId;
        private bool _canUpdateActiveId;


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

        private ItemEntity _navigatedFolderItem = new ItemEntity();
        public ItemEntity NavigatedFolderItem
        {
            get => _navigatedFolderItem;
            set => SetProperty(ref _navigatedFolderItem, value);
        }

        private bool _likeIconVisible = false;
        public bool LikeIconVisible
        {
            get => _likeIconVisible;
            set => SetProperty(ref _likeIconVisible, value);
        }

        private bool _isActiveFolder;
        public bool IsActiveFolder
        {
            get => _isActiveFolder;
            set => SetProperty(ref _isActiveFolder, value);
        }

        private string _likeIcon;
        public string LikeIcon
        {
            get { return NavigatedFolderItem.Liked ? Images.Like : Images.DefaultLike; }
            set
            {
                SetProperty(ref _likeIcon, value);
            }
        }

        public BookletItemDetailsPageViewModel(
            IUserDialogs userDialogs,
            INavigationService navigationService,
            IAppFileSystemService appFileSystemService,
            IAppSettings appSettings,
            IRestService restService,
            IItemDBService itemDBService) : base(navigationService)
        {
            this._userDialogs = userDialogs;
            this._itemDBService = itemDBService;
            this._appFileSystemService = appFileSystemService;
            this.appSettings = appSettings;
            this._restService = restService;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.GetNavigationMode() != NavigationMode.Back)
            {
                NavigatedFolderItem = GetParameters<ItemEntity>(parameters);

                if (NavigatedFolderItem?.ImageByteArray == null && NavigatedFolderItem.Id != 0)
                {
                    NavigatedFolderItem.ImageByteArray = await _restService.GetImageById(NavigatedFolderItem.Id);
                    RaisePropertyChanged(nameof(NavigatedFolderItem));
                    
                }

                _canUpdateActiveId = true;
                IsActiveFolder = appSettings.ActiveFolderId == NavigatedFolderItem.Id;
                RaisePropertyChanged(nameof(IsActiveFolder));

                if (NavigatedFolderItem.LocalId == 0)
                {
                    LikeIconVisible = true;
                    RaisePropertyChanged(nameof(LikeIcon));
                }


                await InitItemsList();
            }
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();

            _canUpdateActiveId = false;
        }

        private async Task InitItemsList()
        {   
            try
            {
                BookletItems.Clear();

                await LoadItems(0, 10);
            }
            catch (Exception ex)
            {
                LogService.Log(ex);
            }
        }

        public async Task LoadItems(int lastItemIndex = 0, int page = 10)
        {
            try
            {
                var res = await _itemDBService.GetItemsByPageWithChildren(
                    NavigatedFolderItem.IsFolder,
                    NavigatedFolderItem.Id,
                    lastItemIndex,
                    page);

                foreach (var item in res)
                {
                    BookletItems.Add(new ItemEntity
                    {
                        LocalId = item.LocalId,
                        Id = item.Id,
                        Description = item.Description,
                        ParentId = item.ParentId,
                        Title = item.Title,
                        IsFolder = item.IsFolder,
                        AudioFileName = item.AudioFileName,
                        //AudioByteArray = StreamHelpers.GetByteArrayFromFilePath(_appFileSystemService.GetFilePath(item.AudioFileName, FileType.Audio)),
                        ImageByteArray = StreamHelpers.GetByteArrayFromFilePath(_appFileSystemService.GetFilePath(item.ImageFileName, FileType.Image))
                    });
                }
            }
            catch (Exception ex)
            {
                LogService.Log(ex);
            }
        }


        public ICommand IsActivePropertyChangedCommand => new Command(() =>
        {
            if (_canUpdateActiveId)
            {
                appSettings.ActiveFolderId = IsActiveFolder ? NavigatedFolderItem.Id : 0;
            }
        });

        public ICommand ItemTappedCommad => new Command<ItemEntity>(async (item) =>
        {
            if (!item.IsFolder)
            {
                await navigationService.NavigateAsync(PageNames.ItemsGalleryPage, CreateParameters(new GalleryNavigationArgs
                {
                    SelectedChildModel = item,
                    NavigatedModel = NavigatedFolderItem,
                }));
            }
            else
            {
                await navigationService.NavigateAsync(PageNames.BookletItemDetailsPage, CreateParameters(item));
            }
        });

        public ICommand LikeCommand => new Command(async () =>
        {
            try
            {
                NavigatedFolderItem.Liked = !NavigatedFolderItem.Liked;
                RaisePropertyChanged(nameof(LikeIcon));

                IsBusy = true;

                if (!NavigatedFolderItem.Liked && NavigatedFolderItem.LocalId != 0)
                {
                    await _itemDBService.DeleateItem(NavigatedFolderItem);
                    NavigatedFolderItem.LocalId = 0;
                }
                else
                {
                    NavigatedFolderItem.ParentId = (NavigatedFolderItem.ParentId == 0 ? NavigatedFolderItem.ParentId : -1);
                    NavigatedFolderItem.LocalId = await _itemDBService.InsertOrUpdateWithChildren(NavigatedFolderItem);

                    await InitParentEntities();

                    appSettings.NeedToUpdateHomePage = true;
                }

                _userDialogs.Toast(AppResources.GalleryUpdated);
            }
            catch (Exception ex)
            {
                LogService.Log(ex);
            }
            finally
            {
                IsBusy = false;
            }
        });

        public ICommand CodeTypingCommand => new Command(async () =>
        {
            await navigationService.NavigateAsync(PageNames.CodeTypingPage, CreateParameters(new CodeTypingNavigationArgs
            {
                ActiveFolderEntity = NavigatedFolderItem,
                PageUpdated = async (item) =>
                {
                    if (item == null)
                    {
                        LikeIconVisible = false;
                        BookletItems.Clear();
                        await LoadItems(0, 10);
                    }
                    else
                    {
                        BookletItems.Add(item);
                    }
                }
            }));
        });

        private async Task InitParentEntities()
        { 
            try
            {
                _ParentItemEntityId = NavigatedFolderItem.ParentId;

                while (_ParentItemEntityId != -1)
                {
                    if (NavigatedFolderItem.Liked)
                    {
                        var parentItemEntity = await _itemDBService.FindItemEntityById(_ParentItemEntityId);
                        if (parentItemEntity == null)
                        {
                            var generalInfoParentItemEntity = await _restService.GetGeneralItemInfo(_ParentItemEntityId);

                            var imageParentItemEntityByteArray = await _restService.GetImageById(_ParentItemEntityId);
                            if (imageParentItemEntityByteArray == null)
                            {
                                await _userDialogs.AlertAsync("I can't find pictures for category with such " + "id: " + NavigatedFolderItem.ParentId, "Not found", "Ok");
                                return;
                            }
                            else
                            {
                                await _itemDBService.InsertOrUpdateWithChildren(new ItemEntity
                                {
                                    Id = _ParentItemEntityId,
                                    Title = generalInfoParentItemEntity.DefaultTitle,
                                    ParentId = generalInfoParentItemEntity.ParentId ?? -1,
                                    ImageByteArray = imageParentItemEntityByteArray,
                                    IsFolder = generalInfoParentItemEntity.IsFolder,
                                });
                            }

                            _ParentItemEntityId = generalInfoParentItemEntity.ParentId.HasValue ? generalInfoParentItemEntity.ParentId.Value : -1;
                        }
                        else
                        {
                            _ParentItemEntityId = -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Log(ex);
            }
        }

        public ICommand DeleteCommand => new Command(async (item) =>
        {
            var model = item as ItemEntity;

            int countOfCategoryItems = (await _itemDBService.GetItemsByParentIdAll(model.Id)).Count();
            var result = await _userDialogs.ConfirmAsync($"Remove {countOfCategoryItems} items?", "Confirmation", "Yes", "No");

            if (!result) return;

            await _itemDBService.DeleateItemWithChildren(model);

            BookletItems.Remove(model);

            appSettings.NeedToUpdateHomePage = true;

            if (BookletItems.Count == 0)
            {
                await _itemDBService.DeleateItem(NavigatedFolderItem);

                await navigationService.GoBackToRootAsync();
                await navigationService.NavigateAsync($"{nameof(HomePage)}");
            }
        });

        
    }
}
