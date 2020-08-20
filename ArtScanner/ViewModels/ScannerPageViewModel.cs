using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using ArtScanner.Models;
using ArtScanner.Models.Entities;
using ArtScanner.Services;
using ArtScanner.Utils.Constants;
using ArtScanner.Utils.Helpers;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace ArtScanner.ViewModels
{
    class ScannerPageViewModel : BaseViewModel
    {
        private IUserDialogs _userDialogs;
        private IRestService _restService;

        public ZXing.Result Result { get; set; }

        public ScannerPageViewModel(
            IRestService restService,
            IUserDialogs userDialogs,
            INavigationService navigationService) : base(navigationService)
        {
            this._userDialogs = userDialogs;
            this._restService = restService;
        }

        public ICommand BackCommand => new Command(async () => { await navigationService.GoBackAsync(); });

        public ICommand QRScanResultCommand => new Command(async () =>
        {
            try
            {
                //var imageByteArray = await _restService.GetImageById(Result?.Text);
                //var musicStream = await _restService.GetMusicStreamById(Result?.Text);
                //var text = await _restService.GetTextById(Result?.Text);

                //if (imageByteArray == null ||
                //    musicStream == null ||
                //    text == null)
                //{
                //    await _userDialogs.AlertAsync("Сould not find by this qr-code...", "Oops", "Ok");
                //    return;
                //}

                //appFileSystemService.SaveStreamAudio(musicStream, Result.Text + ".mp3");

                var foundedItem = new ItemEntity
                {
                    Id = Result?.Text,
                    //ImageByteArray = imageByteArray,
                    //MusicByteArray = musicStream,
                    //Title = text.Substring(0, text.IndexOf(Environment.NewLine)),
                    //Description = text,
                    //MusicFileName = Result.Text + ".mp3",
                };

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await navigationService.NavigateAsync(PageNames.ItemsGalleryDetailsPage, CreateParameters(foundedItem));

                });
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        });
    }
}
