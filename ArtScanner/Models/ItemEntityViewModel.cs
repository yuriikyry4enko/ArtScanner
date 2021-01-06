using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using ArtScanner.Resources;
using ArtScanner.Services;
using ArtScanner.Utils.AuthConfigs;
using ArtScanner.Utils.Constants;
using MediaManager;
using Prism.Navigation;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ArtScanner.Models
{
    class ItemEntityViewModel : BaseModel
    {
        public INavigationService _navigationService { get; set; }
        private readonly IAppFileSystemService _appFileSystemService;

        public ItemEntityViewModel(
            IAppFileSystemService appFileSystemService)
        {
            _appFileSystemService = appFileSystemService;
        }

        #region Properties

        // Server Id
        public long Id { get; set; }
        public long ParentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string MusicUrl { get; set; }
        public string ImageUrl { get; set; }
        public string WikiUrl { get; set; }
        public string LangTag { get; set; }
        public bool Liked { get; set; }
        public string AudioFileName { get; set; }
        public string ImageFileName { get; set; }

        [Ignore]
        public bool IsPlayButtonEnable { get; set; }

        [Ignore]
        public bool IsPlaying { get; set; }

        [Ignore]
        public byte[] ImageByteArray { get; set; }

        [Ignore]
        public byte[] AudioByteArray { get; set; }

        [Ignore]
        public bool firstPlaying { get; set; } = true;

        [Ignore]
        public string PlayIcon
        {
            get { return IsPlaying ? Images.Pause : Images.Play; }
        }

        public string GetAudioFileName()
        {
            return $"{Title.Replace(" ", string.Empty)}{Id}.mp3";
        }

        #endregion

        #region Commands

        public ICommand TwittCommand => new Command(async () =>
        {
            try
            {
                if (OAuthConfig.User == null)
                {
                    OAuthConfig.navigationService = _navigationService;
                    await _navigationService.NavigateAsync(PageNames.ProviderLoginPage);
                }
            }
            catch(Exception ex)
            {
                LogService.Log(ex);
            }
        });

        public ICommand ShareCommand => new Command(async () =>
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = WikiUrl,
                Title = Title
            });
        });

        public ICommand PlayCommand => new Command(async () =>
        {   
            try
            {
                IsPlayButtonEnable = false;

                if (firstPlaying)
                {
                    IsPlaying = false;
                    if (_appFileSystemService.DoesAudioExist(GetAudioFileName()) && Device.RuntimePlatform == Device.Android)
                    {
                        var documentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
                        string localFilename = GetAudioFileName();
                        string localPath = Path.Combine(documentsPath, localFilename);

                        CrossMediaManager.Current.AutoPlay = true;
                        await CrossMediaManager.Current.Play("file://" + localPath);
                    }
                    else
                    {
                        var audioUrl = string.Format(ApiConstants.GetAudioStreamById, LangTag, Id);
                        var mediaItem = await CrossMediaManager.Current.Extractor.CreateMediaItem(audioUrl);
                        mediaItem.MediaType = MediaManager.Library.MediaType.Audio;

                        await CrossMediaManager.Current.Play(mediaItem);
                        CrossMediaManager.Current.Queue.Current.IsMetadataExtracted = false;
                    }

                    firstPlaying = false;
                }

                if (IsPlaying)
                {
                    IsPlaying = false;
                    OnPropertyChanged(nameof(PlayIcon));

                    await CrossMediaManager.Current.Pause();
                }
                else
                {
                    IsPlaying = true;
                    OnPropertyChanged(nameof(PlayIcon));

                    await CrossMediaManager.Current.Play();
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
        });

        #endregion
    }
}
