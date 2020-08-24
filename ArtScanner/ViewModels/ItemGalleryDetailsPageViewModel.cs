using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ArtScanner.Models.Entities;
using ArtScanner.Resources;
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

                    ItemModel.ImageUrl = string.Format(Utils.Constants.ApiConstants.GetJPGById, ItemModel.Id);

                    CrossMediaManager.Current.AutoPlay = false;
                    await CrossMediaManager.Current.Play(string.Format(Utils.Constants.ApiConstants.GetAudioStreamById, ItemModel.Id));

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

        public ICommand BackCommand => new Command(async () => { await navigationService.GoBackAsync(); });

        public ICommand LikeCommand => new Command(async () =>
        {
            try
            {
                ItemModel.Liked = !ItemModel.Liked;
                RaisePropertyChanged(nameof(LikeIcon));

                if (!ItemModel.Liked && ItemModel.LocalId != 0)
                {
                    await _itemDBService.DeleateItem(ItemModel);
                    ItemModel.LocalId = 0;
                }
                else
                {
                    var imageByteArray = await _restService.GetImageById(ItemModel.Id);
                    ItemModel.ImageByteArray = imageByteArray;

                    var resultId = await _itemDBService.InsertOrUpdateWithChildren(ItemModel);
                    ItemModel.LocalId = resultId;
                }
                _userDialogs.Toast("Gallery updated");
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

        private async Task LoadAndInitItemModel()
        {
            try
            {
                var text = await _restService.GetTextById(ItemModel.Id);
                if (text == null)
                {
                    await _userDialogs.AlertAsync("Сould not find by this qr-code...", "Oops", "Ok");
                    await navigationService.GoBackAsync();
                    return;
                }

                ItemModel.Title = text.Substring(0, text.IndexOf(Environment.NewLine));
                ItemModel.Description = text;

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
