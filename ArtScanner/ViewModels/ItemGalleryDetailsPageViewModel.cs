using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ArtScanner.Models.Entities;
using ArtScanner.Resources;
using ArtScanner.Resx;
using ArtScanner.Services;
using ArtScanner.Utils.AuthConfigs;
using ArtScanner.Utils.Constants;
using ArtScanner.Utils.Helpers;
using MediaManager;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;
using NavigationMode = Prism.Navigation.NavigationMode;

namespace ArtScanner.ViewModels
{
    class ItemGalleryDetailsPageViewModel : BaseViewModel
    {
        private IUserDialogs _userDialogs;
        private IItemDBService _itemDBService;
        private IAppFileSystemService _appFileSystemService;
        private IRestService _restService;

        #region Properties

        private ItemEntity _itemModel = new ItemEntity();
        public ItemEntity ItemModel
        {
            get { return _itemModel; }
            set
            {
                SetProperty(ref _itemModel, value);
                RaisePropertyChanged(nameof(LikeIcon));
            }
        }

        private bool isPlaying;
        public bool IsPlaying
        {
            get { return isPlaying; }
            set
            {
                SetProperty(ref isPlaying, value);
                RaisePropertyChanged(nameof(PlayIcon));
            }
        }

        private string _playIcon;
        public string PlayIcon
        {
            get { return isPlaying ? Images.Pause : Images.Play; }
            set
            {
                SetProperty(ref _playIcon, value);
            }
        }

        private bool _isActivityLoad;
        public bool IsActivityLoad
        {
            get { return _isActivityLoad; }
            set { SetProperty(ref _isActivityLoad, value); }
        }

        private bool _isPlayButtonEnable = true;
        public bool IsPlayButtonEnable
        {
            get { return _isPlayButtonEnable; }
            set { SetProperty(ref _isPlayButtonEnable, value); }
        }


        private string _likeIcon;
        public string LikeIcon
        {
            get { return ItemModel.Liked ? Images.Like : Images.DefaultLike; }
            set
            {
                SetProperty(ref _likeIcon, value);
            }
        }

        #endregion

        public ItemGalleryDetailsPageViewModel(
            INavigationService navigationService,
            IItemDBService itemDBService,
            IRestService restService,
            IAppFileSystemService appFileSystemService,
            IUserDialogs userDialogs) : base(navigationService)
        {
            this._userDialogs = userDialogs;
            this._itemDBService = itemDBService;
            this._appFileSystemService = appFileSystemService;
            this._restService = restService;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            try
            {
                if (parameters.GetNavigationMode() != NavigationMode.Back)
                {
                    ItemModel = GetParameters<ItemEntity>(parameters);
                       
                    RaisePropertyChanged(nameof(ItemModel));

                    IsPlayButtonEnable = false;
                    IsBusy = true;

                    ItemModel.ImageByteArray = await _restService.GetImageById(ItemModel.LangTag, ItemModel.Id);

                    CrossMediaManager.Current.AutoPlay = false;
                    await CrossMediaManager.Current.Play(string.Format(Utils.Constants.ApiConstants.GetAudioStreamById, ItemModel.LangTag, ItemModel.Id));

                    if (ItemModel.LocalId == 0)
                    {
                        await CheckForItemExistedInLocalDB();
                    }

                    IsBusy = false;
                    IsPlayButtonEnable = true;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task CheckForItemExistedInLocalDB()
        {
            try
            {
                var itemEntity = await _itemDBService.GetByServerId(ItemModel.Id);

                if (itemEntity != null)
                {
                    ItemModel.Liked = true;
                    ItemModel = itemEntity;
                    ItemModel.ImageByteArray = StreamHelpers.GetByteArrayFromFilePath(_appFileSystemService.GetFilePath(ItemModel.ImageFileName));
                }
                else
                {
                    await LoadAndInitItemModel();
                }

                RaisePropertyChanged(nameof(ItemModel));
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();

            CrossMediaManager.Current.Stop();
        }

        #region Commands

        public ICommand PlayCommand => new DelegateCommand(Play).ObservesCanExecute(() => IsPlayButtonEnable);

        public ICommand LikeCommand => new Command(async () =>
        {
            try
            {
                ItemModel.Liked = !ItemModel.Liked;
                RaisePropertyChanged(nameof(LikeIcon));

                IsBusy = true;

                if (!ItemModel.Liked && ItemModel.LocalId != 0)
                {
                    await _itemDBService.DeleateItem(ItemModel);
                    ItemModel.LocalId = 0;
                }
                else
                {
                    var imageByteArray = await _restService.GetImageById(ItemModel.LangTag, ItemModel.Id);
                    ItemModel.ImageByteArray = imageByteArray;

                    var resultId = await _itemDBService.InsertOrUpdateWithChildren(ItemModel);
                    ItemModel.LocalId = resultId;

                    await InitParentEntities();
                }

                IsBusy = false;

                _userDialogs.Toast(AppResources.GalleryUpdated);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }

        });

        public ICommand TwittCommand => new Command(async () =>
        {
            if (OAuthConfig.User == null)
            {
                OAuthConfig.navigationService = navigationService;
                await navigationService.NavigateAsync(PageNames.ProviderLoginPage);
            }
        });

        public ICommand ShareCommand => new Command(async () =>
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = ItemModel.WikiUrl,
                Title = ItemModel.Title
            });
        });

        #endregion

        private async void Play()
        {
            try
            {
                IsPlayButtonEnable = false;

                await CrossMediaManager.Current.Play();

                if (isPlaying)
                {
                    await CrossMediaManager.Current.Pause();
                    IsPlaying = false;
                }
                else
                {
                    await CrossMediaManager.Current.Play();
                    IsPlaying = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsPlayButtonEnable = true;
            }
        }

        private async Task InitParentEntities()
        {
            try
            {
                if (ItemModel.Liked)
                {
                    var categoryItem = await _itemDBService.FindCategoryById(ItemModel.ParentId);
                    if(categoryItem == null)
                    {
                        var category = await _restService.GetGeneralItemInfo(ItemModel.ParentId);

                        var imageCategoryByteArray = await _restService.GetImageById(ItemModel.LangTag, ItemModel.ParentId.ToString());
                        if (imageCategoryByteArray == null)
                        {
                            await _userDialogs.AlertAsync("I can't find pictures for category with such " + "id: " + ItemModel.ParentId, "Oops", "Ok");
                            return;
                        }
                        else
                        {
                            await _itemDBService.InsertOrUpdateCategoryWithChildren(new CategoryItemEntity { Id = ItemModel.ParentId, Title = category.DefaultTitle, ParentId = category.ParentId, ImageByteArray = imageCategoryByteArray, });
                        }


                        var folderItem = await _itemDBService.FindFolderById(category.ParentId);
                        if (folderItem == null)
                        {
                            var folder = await _restService.GetGeneralItemInfo(category.ParentId);

                            var imageFolderByteArray = await _restService.GetImageById(ItemModel.LangTag, category.ParentId.ToString());
                            if(imageFolderByteArray == null)
                            {
                                await _userDialogs.AlertAsync("I can't find pictures for folder with such " + "id: " + ItemModel.ParentId, "Oops", "Ok");

                            }
                            else
                            {
                                await _itemDBService.InsertOrUpdateFolderWithChildren(new FolderItemEntity { Id = category.ParentId, Title = folder.DefaultTitle, ImageByteArray = imageFolderByteArray, });
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task LoadAndInitItemModel()
        {
            try
            {
                var textModel = await _restService.GetTextById(ItemModel.LangTag, ItemModel.Id);
                if (textModel == null)
                {
                    await _userDialogs.AlertAsync("Сould not find by this qr-code...", "Oops", "Ok");
                    await navigationService.GoBackAsync();
                    return;
                }

                ItemModel.Title = textModel.Title;
                ItemModel.Description = textModel.Description;

                //TODO:need to find better solution for reload text before img smoothly
                RaisePropertyChanged(nameof(ItemModel));

             
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
