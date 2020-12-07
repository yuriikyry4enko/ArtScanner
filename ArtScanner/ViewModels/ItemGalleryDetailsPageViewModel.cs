using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ArtScanner.Models;
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
        private IAppSettings _appSettings;

        private long _ParentItemEntityId;
        private bool _firstTouchPlayButton = true;

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
            IAppSettings appSettings,
            IAppFileSystemService appFileSystemService,
            IUserDialogs userDialogs) : base(navigationService)
        {
            this._userDialogs = userDialogs;
            this._itemDBService = itemDBService;
            this._appFileSystemService = appFileSystemService;
            this._restService = restService;
            this._appSettings = appSettings;
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

                    if (ItemModel.LocalId == 0)
                    {
                        await CheckForItemExistedInLocalDB();
                    }

                    CrossMediaManager.Current.AutoPlay = false;
                    await CrossMediaManager.Current.Play(string.Format(ApiConstants.GetAudioStreamById, ItemModel.LangTag, ItemModel.Id));


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
                    ItemModel.ImageByteArray = StreamHelpers.GetByteArrayFromFilePath(_appFileSystemService.GetFilePath(ItemModel.ImageFileName, FileType.Image));
                    //ItemModel.AudioByteArray = StreamHelpers.GetByteArrayFromFilePath(_appFileSystemService.GetFilePath(ItemModel.AudioFileName, FileType.Audio));

                    RaisePropertyChanged(nameof(ItemModel));
                }
                else
                {
                    await LoadTextInfoItemModel();

                    ItemModel.ImageByteArray = await _restService.GetImageById(ItemModel.Id);

                    RaisePropertyChanged(nameof(ItemModel));
                }
            }
            catch(Exception ex)
            {
                LogService.Log(ex);
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
                    var resultId = await _itemDBService.InsertOrUpdateWithChildren(ItemModel);
                    ItemModel.LocalId = resultId;
                    
                    await InitParentEntities();

                    await _itemDBService.SaveAudioFileFromStream(ItemModel);
                }

                _userDialogs.Toast(AppResources.GalleryUpdated);
            }
            catch(Exception ex)
            {
                LogService.Log(ex);
            }
            finally
            {
                IsBusy = false;
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
                Title = ItemModel.Title,
                Text = ItemModel.Description,
                Uri = ItemModel.ImageUrl,
            });
        });

        public ICommand ChangeLanguageCommand => new Command(async () =>
        {
            try
            {
                GeneralItemInfoModel result = await _restService.GetGeneralItemInfo(ItemModel.Id);

                await navigationService.NavigateAsync(PageNames.ApologizeLanguagePopupPage, CreateParameters(new ApologizeNavigationArgs
                {
                    LanguageTags = result.Languages,
                    PopupResultAction = async (string langTagSelected) =>
                    {
                       
                    },
                    PageApologizeFinishedLoading = () =>
                    {
                        IsBusy = false;
                    }
                }));
            }
            catch(Exception ex)
            {
                LogService.Log(ex);
            }
        });


        #endregion

        private async void Play()
        {
            try
            {
              
                IsPlayButtonEnable = false;

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


                if (ItemModel.LocalId == 0 && _firstTouchPlayButton)
                {
                    _firstTouchPlayButton = false;
                    ItemModel.AudioStream = await _restService.GetMusicStreamById(ItemModel.Id, ItemModel.LangTag);
                }

            }
            catch (Exception ex)
            {
                LogService.Log(ex);
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
                _ParentItemEntityId = ItemModel.ParentId;

                while (_ParentItemEntityId != -1)
                {
                    if (ItemModel.Liked)
                    {
                        var parentItemEntity = await _itemDBService.FindItemEntityById(_ParentItemEntityId);
                        if (parentItemEntity == null)
                        {
                            var generalInfoParentItemEntity = await _restService.GetGeneralItemInfo(_ParentItemEntityId);

                            var imageParentItemEntityByteArray = await _restService.GetImageById(_ParentItemEntityId);
                            if (imageParentItemEntityByteArray == null)
                            {
                                await _userDialogs.AlertAsync("I can't find pictures for category with such " + "id: " + ItemModel.ParentId, "Not found", "Ok");
                                return;
                            }
                            else
                            {
                                if(generalInfoParentItemEntity.IsFolder)
                                    _appSettings.NeedToUpdateHomePage = true;

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
            catch(Exception ex)
            {
                LogService.Log(ex);
            }
        }

        private async Task LoadTextInfoItemModel()
        {
            try
            {
                var textModel = await _restService.GetTextById(ItemModel.LangTag, ItemModel.Id);
                if (textModel == null)
                {
                    await _userDialogs.AlertAsync("Text for your language tag preferences was not found...", "Not found", "Ok");
                    await navigationService.GoBackAsync();
                    return;
                }

                ItemModel.Description = textModel.Description;

                RaisePropertyChanged(nameof(ItemModel));
            }
            catch (Exception ex)
            {
                LogService.Log(ex);
            }
        }
    }
}
