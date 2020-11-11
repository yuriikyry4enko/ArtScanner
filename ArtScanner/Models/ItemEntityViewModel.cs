using System;
using System.Diagnostics;
using System.Windows.Input;
using ArtScanner.Utils.AuthConfigs;
using ArtScanner.Utils.Constants;
using MediaManager;
using Prism.Navigation;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ArtScanner.Models
{
    public class ItemEntityViewModel : BaseModel
    {
        public INavigationService _navigationService { get; set; }

        // Server Id
        public string Id { get; set; }

        public long ParentId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string MusicUrl { get; set; }
        public string ImageUrl { get; set; }
        public string WikiUrl { get; set; }


        public string LangTag { get; set; }

        public bool Liked { get; set; }

        [Ignore]
        public bool IsPlayButtonEnable { get; set; }

        [Ignore]
        public bool IsPlaying { get; set; }

        [Ignore]
        public byte[] ImageByteArray { get; set; }

        [Ignore]
        public byte[] MusicByteArray { get; set; }

        public string MusicFileName { get; set; }

        public string ImageFileName { get; set; }

        public ICommand TwittCommand => new Command(async () =>
        {
            if (OAuthConfig.User == null)
            {
                OAuthConfig.navigationService = _navigationService;
                await _navigationService.NavigateAsync(PageNames.ProviderLoginPage);
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

                await CrossMediaManager.Current.Play();

                if (IsPlaying)
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
        });

    }
}
