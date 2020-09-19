using System;
using System.Diagnostics;
using System.Linq;
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
            IsScannerEnabled = true;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            IsScannerEnabled = true;
        }

        #region Commands


        public ICommand QRScanResultCommand => new Command(() =>
        {
            try
            {
                IsBusy = true;

                if (IsScannerEnabled)
                {
                    var foundedItem = new ItemEntity
                    {
                        Id = Result?.Text,
                    };

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        GeneralItemInfoModel result = await _restService.GetGeneralItemInfo(foundedItem.Id);

                        if(result == null)
                        {
                            IsBusy = false;
                            await _userDialogs.AlertAsync(AppResources.СouldNotFindQRCODE, "Oops", "Ok");
                            return;
                        }

                        var langPrefs = await _baseDBService.GetAllAsync<LangPreferencesItemEntity>();
                        var intersectList = result.Languages.Where(x => langPrefs.Any(y => x == y.LangTag)).ToList();

                        if (intersectList.Count() > 0)
                        {
                            foundedItem.LangTag = intersectList.FirstOrDefault();
                            await navigationService.NavigateAsync(PageNames.ItemsGalleryDetailsPage, CreateParameters(foundedItem));
                        }
                        else
                        {
                            await navigationService.NavigateAsync(PageNames.ApologizeLanguagePopupPage, CreateParameters(new ApologizeNavigationArgs
                            {
                                LanguageTags = result.Languages,
                                PopupResultAction = async (string langTagSelected) =>
                                {
                                    if(!string.IsNullOrEmpty(langTagSelected))
                                    {
                                        foundedItem.LangTag = langTagSelected;
                                        await navigationService.NavigateAsync(PageNames.ItemsGalleryDetailsPage, CreateParameters(foundedItem));
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
                    });

                    IsScannerEnabled = false;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        });

        #endregion
    }
}
