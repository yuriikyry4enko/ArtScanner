using System;
using System.Collections.Generic;
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
using Xamarin.Essentials;
using Xamarin.Forms;
using NavigationMode = Prism.Navigation.NavigationMode;

namespace ArtScanner.ViewModels
{
    class ScannerPageViewModel : BaseViewModel
    {
        private IUserDialogs _userDialogs { get; set; }
        private IRestService _restService { get; set; }
        private IBaseDBService _baseDBService { get; set; }
        private NetworkAccess _networkAccess { get; set; }

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
            _networkAccess = Connectivity.NetworkAccess;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.GetNavigationMode() == NavigationMode.Back)
            {
                var _NavBackArgs = GetParameters<LoadingNavigationArgs>(parameters);
                if (_NavBackArgs.IsCanceled)
                    IsScannerEnabled = true;
            }
        }

        #region Commands

        public ICommand QRScanResultCommand => new Command(() =>
        {
            
            IsBusy = true;
            bool isCanceled = false;

            if (IsScannerEnabled)
            {
                IsScannerEnabled = false;

                var foundedItem = new ItemEntity
                {
                    //Id = Int64.Parse(Result?.Text),
                    Id = 719,
                };

                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {

                        if (_networkAccess != NetworkAccess.Internet)
                        {
                            await _userDialogs.AlertAsync("No internet connection", "Network error", "Ok");
                            IsScannerEnabled = true;
                            return;
                        }

                        string intersectFirstItem = string.Empty;
                        List<LangPreferencesItemEntity> langPrefs;

                        await navigationService.NavigateAsync(PageNames.LoadingPopupPage, CreateParameters(new LoadingNavigationArgs()
                        {
                            PageLoadingCanceled = async () =>
                            {
                                isCanceled = true;
                                IsScannerEnabled = true;
                                await Task.CompletedTask;
                                return;
                            },
                        }));

                        GeneralItemInfoModel result = await _restService.GetGeneralItemInfo(foundedItem.Id);

                        if (result == null)
                        {
                            await _userDialogs.AlertAsync(AppResources.СouldNotFindQRCODE + "id: " + foundedItem.Id, "Oops", "Ok");
                            await navigationService.GoBackAsync();

                            return;
                        }

                        if (!result.IsFolder)
                        {
                            langPrefs = await _baseDBService.GetAllAsync<LangPreferencesItemEntity>();
                            intersectFirstItem = result.Languages.FirstOrDefault(x => langPrefs.Any(y => x == y.LangTag));
                        }

                        if (!isCanceled)
                        {
                            if (!string.IsNullOrEmpty(intersectFirstItem) || result.IsFolder)
                            {
                                foundedItem.LangTag = intersectFirstItem;
                                foundedItem.ParentId = result.ParentId.HasValue ? result.ParentId.Value : -1;
                                foundedItem.IsFolder = result.IsFolder;
                                foundedItem.Title = result.DefaultTitle;

                                if (foundedItem.IsFolder)
                                {
                                    await navigationService.NavigateAsync(PageNames.BookletItemDetailsPage, CreateParameters(foundedItem));
                                }
                                else
                                {
                                    await navigationService.NavigateAsync(PageNames.ItemsGalleryDetailsPage, CreateParameters(new ItemGalleryDetailsNavigationArgs
                                    {
                                        ItemModel = foundedItem,
                                        NeedsToUpdatePrevious = () =>
                                        {

                                        }
                                    }));

                                }
                            }
                            else
                            {
                                //Close Loading popup
                                await navigationService.GoBackAsync();

                                await navigationService.NavigateAsync(PageNames.ApologizeLanguagePopupPage, CreateParameters(new ApologizeNavigationArgs
                                {
                                    LanguageTags = result.Languages,
                                    PopupResultAction = async (string langTagSelected) =>
                                    {
                                        if (!string.IsNullOrEmpty(langTagSelected))
                                        {
                                            foundedItem.ParentId = result.ParentId.Value;
                                            foundedItem.LangTag = langTagSelected;
                                            foundedItem.IsFolder = result.IsFolder;
                                            foundedItem.Title = result.DefaultTitle;

                                            if (result.IsFolder)
                                            {
                                                await navigationService.NavigateAsync(PageNames.BookletItemDetailsPage, CreateParameters(foundedItem));
                                            }
                                            else
                                            {
                                                await navigationService.NavigateAsync(PageNames.ItemsGalleryDetailsPage, CreateParameters(new ItemGalleryDetailsNavigationArgs
                                                {
                                                    ItemModel = foundedItem,
                                                    NeedsToUpdatePrevious = () =>
                                                    {

                                                    }
                                                }));

                                            }
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
                        
                    }
                    catch (Exception ex)
                    {
                        LogService.Log(ex);
                    }

                });
            }

            IsScannerEnabled = false;
        });

        #endregion
    }
}
