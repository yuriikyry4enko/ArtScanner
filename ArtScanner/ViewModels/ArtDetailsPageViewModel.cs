﻿using System;
using System.Diagnostics;
using System.Windows.Input;
using ArtScanner.Resources;
using MediaManager;
using Prism.Navigation;
using Xamarin.Forms;
using Prism.Commands;
using ArtScanner.Models;
using Xamarin.Essentials;
using Acr.UserDialogs;
using ArtScanner.Services;
using LinqToTwitter;
using System.Linq;

namespace ArtScanner.ViewModels
{
    class ArtDetailsPageViewModel : BaseViewModel
    {
        #region Services

        private IUserDialogs _userDialogs;
        private ITwitterService _twitterService;

        #endregion

        #region Properties

        private bool _firstLook = false;

        private ArtModel _currentArtModel = new ArtModel();
        public ArtModel CurrentArtModel
        {
            get { return _currentArtModel; }
            set { SetProperty(ref _currentArtModel, value); }
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
            get { return _isLike ; }
            set
            {
                SetProperty(ref _isLike, value);
                RaisePropertyChanged(nameof(LikeIcon));
            }
        }

        public string LikeIcon => _isLike ? Images.Like : Images.DefaultLike;

        #endregion

        public ArtDetailsPageViewModel(
            ITwitterService twitterService,
            INavigationService navigationService,
            IUserDialogs userDialogs) : base(navigationService)
        {
            this._twitterService = twitterService;
            this._userDialogs = userDialogs;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters != null)
                CurrentArtModel = GetParameters<ArtModel>(parameters);
        }

        #region Commands

        public ICommand PlayCommand => new DelegateCommand(Play).ObservesCanExecute(() => IsPlayButtonEnable);

        public ICommand BackCommand => new Command(async () => { await navigationService.GoBackAsync(); });

        public ICommand LikeCommand => new Command(async () =>
        {
            //IsLike = !IsLike;
            await _userDialogs.ConfirmAsync("Not implemented", "Oops", "Ok");
        });
        public ICommand TwittCommand => new Command(() =>
        {
            //var auth = new ApplicationOnlyAuthorizer()
            //{
            //    CredentialStore = new InMemoryCredentialStore
            //    {
            //        ConsumerKey = "9rD8HiNF8pvfGeZAeSL18DybE",
            //        ConsumerSecret = "PiwYPbE8P1TLGJjdbD8zeer0vMHydYw3qSDA6XC60cvmqt92tC",
            //    },
            //};
            //await auth.AuthorizeAsync();

            //var ctx = new TwitterContext(auth);

            //Search searchResponse = await
            //    (from search in ctx.Search
            //     where search.Type == SearchType.Search &&
            //           search.Query == "\"Twitter\""
            //     select search)
            //    .SingleAsync();

            //var Tweets =
            //    (from tweet in searchResponse.Statuses
            //     select new Tweet
            //     {
            //         StatusID = tweet.StatusID,
            //         ScreenName = tweet.User.ScreenNameResponse,
            //         Text = tweet.Text,
            //         ImageUrl = tweet.User.ProfileImageUrl
            //     })
            //    .ToList();

            //await _userDialogs.ConfirmAsync("Not implemented", "Oops", "Ok");
        });


        public ICommand ShareCommand => new Command(async () =>
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = CurrentArtModel.WikiUrl,
                Title = CurrentArtModel.Title
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

                    await CrossMediaManager.Current.Play(CurrentArtModel.MusicUrl);

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

    public class Tweet
    {
        public ulong StatusID { get; set; }

        public string ScreenName { get; set; }

        public string Text { get; set; }

        public string ImageUrl { get; set; }
    }
}
