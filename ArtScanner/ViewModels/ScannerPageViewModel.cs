using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ArtScanner.Models;
using ArtScanner.Models.Entities;
using ArtScanner.Resx;
using ArtScanner.Services;
using ArtScanner.Utils.Constants;
using Prism.Navigation;
using Xamarin.Forms;

namespace ArtScanner.ViewModels
{
    class ScannerPageViewModel : BaseViewModel
    {
        private IUserDialogs _userDialogs { get; set; }
        private IRestService _restService { get; set; }
        private IBaseDBService _baseDBService { get; set; }

        public ZXing.Result Result { get; set; }
        private bool IsScannerEnabled { get; set; } = true;

        public ScannerPageViewModel(
            IUserDialogs userDialogs,
            IBaseDBService baseDBService,
            IRestService restService,
            INavigationService navigationService) : base(navigationService)
        {
            this._restService = restService;
            this._baseDBService = baseDBService;
            this._userDialogs = userDialogs;
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            IsScannerEnabled = true;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.GetNavigationMode() == NavigationMode.Back)
            {
                IsScannerEnabled = true;
            }
        }

        #region Commands

        public ICommand QRScanResultCommand => new Command(() =>
        {
            try
            {
                IsBusy = true;
                bool isCanceled = false;

                if (IsScannerEnabled)
                {
                    var foundedItem = new ItemEntity
                    {
                        Id = Result?.Text,
                    };

                   Device.BeginInvokeOnMainThread(async () =>
                    {
                        await navigationService.NavigateAsync(PageNames.LoadingPopupPage, CreateParameters(new LoadingNavigationArgs()
                        {
                            PageLoadingCanceled = async () =>
                            {
                                isCanceled = true;
                                await Task.CompletedTask;
                                return;
                            },
                        }));

                        //var qrcodeData = await _restService.GetIdByQRCode(foundedItem.Id);

                        //if (qrcodeData == null)
                        //{
                        //    await _userDialogs.AlertAsync(AppResources.СouldNotFindQRCODE + "id: " + foundedItem.Id, "Oops", "Ok");
                        //    return;
                        //}

                        GeneralItemInfoModel result = await _restService.GetGeneralItemInfo(foundedItem.Id);

                        if(result == null)
                        {
                            await _userDialogs.AlertAsync(AppResources.СouldNotFindQRCODE + "id: " + foundedItem.Id, "Oops", "Ok");
                            await navigationService.GoBackAsync();

                            return;
                        }

                        var langPrefs = await _baseDBService.GetAllAsync<LangPreferencesItemEntity>();
                        var intersectFirstItem = result.Languages.FirstOrDefault(x => langPrefs.Any(y => x == y.LangTag));

                        if (!isCanceled)
                        {
                            if (intersectFirstItem != null)
                            {
                                foundedItem.LangTag = intersectFirstItem;
                                foundedItem.ParentId = result.ParentId;
                                await navigationService.NavigateAsync(PageNames.ItemsGalleryDetailsPage, CreateParameters(foundedItem));
                            }
                            else
                            {
                                await navigationService.NavigateAsync(PageNames.ApologizeLanguagePopupPage, CreateParameters(new ApologizeNavigationArgs
                                {
                                    LanguageTags = result.Languages,
                                    PopupResultAction = async (string langTagSelected) =>
                                    {
                                        if (!string.IsNullOrEmpty(langTagSelected))
                                        {
                                            foundedItem.ParentId = result.ParentId;
                                            foundedItem.LangTag = langTagSelected;
                                            await navigationService.NavigateAsync(PageNames.ItemsGalleryDetailsPage, CreateParameters(foundedItem));
                                            IsBusy = false;
                                        }
                                        else
                                        {
                                            IsScannerEnabled = true;
                                        }
                                    },
                                    PageApologizeFinishedLoading = () =>
                                    {
                                        IsBusy = false;
                                    }
                                }));
                            }
                        }
                        else
                        {
                            isCanceled = false;
                        }
                    });
                }

                IsScannerEnabled = false;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        });

        #endregion
    }
}
