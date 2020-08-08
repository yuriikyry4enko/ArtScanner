using System;
using System.Diagnostics;
using System.Windows.Input;
using Acr.UserDialogs;
using ArtScanner.Models;
using ArtScanner.Resources;
using ArtScanner.Services;
using ArtScanner.Utils.AuthConfigs;
using ArtScanner.Utils.Constants;
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

        #region Properties

        private bool _firstLook = false;

        private ArtModel _itemModel = new ArtModel();
        public ArtModel ItemModel
        {
            get { return _itemModel; }
            set { SetProperty(ref _itemModel, value); }
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


        private bool _isLike;
        public bool IsLike
        {
            get { return _isLike; }
            set
            {
                SetProperty(ref _isLike, value);
                RaisePropertyChanged(nameof(LikeIcon));
            }
        }

        public string LikeIcon => _isLike ? Images.Like : Images.DefaultLike;

        #endregion

        public ItemGalleryDetailsPageViewModel(
            INavigationService navigationService,
            IItemDBService itemDBService,
            IUserDialogs userDialogs) : base(navigationService)
        {
            this._userDialogs = userDialogs;
            this._itemDBService = itemDBService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.GetNavigationMode() != NavigationMode.Back)
            {
                ItemModel = GetParameters<ArtModel>(parameters);
            }
        }


        #region Commands

        public ICommand PlayCommand => new DelegateCommand(Play).ObservesCanExecute(() => IsPlayButtonEnable);

        public ICommand BackCommand => new Command(async () => { await navigationService.GoBackAsync(); });

        public ICommand LikeCommand => new Command(async () =>
        {
            await _userDialogs.ConfirmAsync("Not implemented", "Oops", "Ok");
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

                if (!_firstLook)
                {
                    IsActivityLoad = true;

                    IsPlaying = true;

                    await CrossMediaManager.Current.Play(ItemModel.MusicUrl);

                    _firstLook = true;

                    IsActivityLoad = false;
                }
                else
                {
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
    }

}
