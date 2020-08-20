using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ArtScanner.Models;
using ArtScanner.Models.Entities;
using ArtScanner.Resources;
using ArtScanner.Services;
using ArtScanner.Utils.AuthConfigs;
using ArtScanner.Utils.Constants;
using ArtScanner.Utils.Helpers;
using Plugin.SimpleAudioPlayer;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;
using NavigationMode = Prism.Navigation.NavigationMode;

namespace ArtScanner.ViewModels
{
    class ItemGalleryDetailsPageViewModel : BaseViewModel
    {
        private ISimpleAudioPlayer player;
        private IUserDialogs _userDialogs;
        private IItemDBService _itemDBService;
        private IAppFileSystemService _appFileSystemService;
        private IRestService _restService;

        #region Properties

        private ItemEntity _itemModel = new ItemEntity();
        public ItemEntity ItemModel
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


        private string _likeIcon;
        public string LikeIcon
        {
            get { return ItemModel.Liked ? Images.Like : Images.BackArrow; }
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

            player = CrossSimpleAudioPlayer.Current;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            try
            {
                if (parameters.GetNavigationMode() != NavigationMode.Back)
                {
                    ItemModel = GetParameters<ItemEntity>(parameters);

                    await Task.Run(async () =>
                    {
                        IsPlayButtonEnable = false;

                        if (ItemModel.LocalId > 0)
                        {
                            player.Load(new MemoryStream(StreamHelpers.GetByteArrayFromFilePath(_appFileSystemService.GetFilePath(ItemModel.MusicFileName))));

                            ItemModel.ImageByteArray = StreamHelpers.GetByteArrayFromFilePath(_appFileSystemService.GetFilePath(ItemModel.ImageFileName));
                        }
                        else
                        {

                            await LoadAndInitItemModel();
                        }

                        await Task.CompletedTask;

                        IsPlayButtonEnable = true;

                    });

                    RaisePropertyChanged(nameof(LikeIcon));
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            try
            {
                player.Pause();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
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
                    //var itemById = await _itemDBService.GetByIdWithChildren(Int64.Parse(ItemModel.Id));

                    //if (itemById == null)
                    //{
                        var resultId = await _itemDBService.InsertOrUpdateWithChildren(ItemModel);
                        ItemModel.LocalId = resultId;
                    //}
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

        private void Play()
        {
            try
            {
                IsPlayButtonEnable = false;

                if (isPlaying)
                {
                    player.Pause();
                    IsPlaying = false;
                }
                else
                {
                    player.Play();
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
                var musicByteArray = await _restService.GetMusicStreamById(ItemModel.Id);

                if (musicByteArray != null)
                {
                    player.Load(new MemoryStream(musicByteArray));
                }

                var imageByteArray = await _restService.GetImageById(ItemModel.Id);
                var text = await _restService.GetTextById(ItemModel.Id);

                if (imageByteArray == null ||
                    text == null)
                {
                    await _userDialogs.AlertAsync("Сould not find by this qr-code...", "Oops", "Ok");
                    await navigationService.GoBackAsync();
                    return;
                }

                ItemModel.ImageByteArray = imageByteArray;
                ItemModel.Title = text.Substring(0, text.IndexOf(Environment.NewLine));
                ItemModel.Description = text;
                ItemModel.MusicByteArray = musicByteArray;
                ItemModel.MusicFileName = ItemModel.Id + ".mp3";

                RaisePropertyChanged(nameof(ItemModel));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
