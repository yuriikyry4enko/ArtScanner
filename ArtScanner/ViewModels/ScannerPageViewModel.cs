using System;
using System.Diagnostics;
using System.Windows.Input;
using ArtScanner.Models.Entities;
using ArtScanner.Utils.Constants;
using Prism.Navigation;
using Xamarin.Forms;

namespace ArtScanner.ViewModels
{
    class ScannerPageViewModel : BaseViewModel
    {
        public ZXing.Result Result { get; set; }

        public ScannerPageViewModel(
            INavigationService navigationService) : base(navigationService)
        {
        }

        #region Commands

        public ICommand BackCommand => new Command(async () =>
        {
            await navigationService.GoBackAsync();
        });

        public ICommand QRScanResultCommand => new Command(() =>
        {
            try
            {
                var foundedItem = new ItemEntity
                {
                    Id = Result?.Text,
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

        #endregion
    }
}
