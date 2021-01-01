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

namespace ArtScanner.ViewModels
{
    class CodeTypingPageViewModel : BaseViewModel
    {
        private readonly IAppSettings _appSettings;
        private readonly IRestService _restService;
        private readonly IUserDialogs _userDialogs;
        private readonly IBaseDBService _baseDBService;
        private NetworkAccess _networkAccess { get; set; }

        private CodeTypingNavigationArgs CodeTypingNavigationArgs;

        public CodeTypingPageViewModel(
            INavigationService navigationService,
            IAppSettings appSettings,
            IBaseDBService baseDBService,
            IUserDialogs userDialogs,
            IRestService restService) : base(navigationService)
        {
            this._restService = restService;
            this._appSettings = appSettings;
            this._userDialogs = userDialogs;
            this._baseDBService = baseDBService;

            _networkAccess = Connectivity.NetworkAccess;

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.GetNavigationMode() != Prism.Navigation.NavigationMode.Back)
            {
                CodeTypingNavigationArgs = GetParameters<CodeTypingNavigationArgs>(parameters);
            }
        }

        private bool _isAlertMessageVisible;
        public bool IsAlertMessageVisible
        {
            get { return _isAlertMessageVisible; }
            set { SetProperty(ref _isAlertMessageVisible, value); }
        }

        private string _itemCode;
        public string ItemCode
        {
            get { return _itemCode; }
            set { SetProperty(ref _itemCode, value); }
        }


        private string _alertMessage = "There is no item with this code for this folder";
        public string AlertMessage
        {
            get { return _alertMessage; }
            set { SetProperty(ref _alertMessage, value); }
        }

        public ICommand ClearCodeCommand => new Command(() =>
        {
            ItemCode = string.Empty;
        });

        public ICommand ContinueCommand => new Command(async () =>
        {
            try
            {
                string intersectFirstItem = string.Empty;
                List<LangPreferencesItemEntity> langPrefs;
                GeneralItemInfoModel generalItemInfoModel;

                if (string.IsNullOrWhiteSpace(ItemCode)
                  || ItemCode.Length < 3)
                {
                    return;
                }

                if (_networkAccess != NetworkAccess.Internet)
                {
                    await _userDialogs.AlertAsync("No internet connection", "Network error", "Ok");
                    return;
                }

                IsBusy = true;
                bool isCanceled = false;

                var foundedItemId = await _restService.GetItemIdByShortCode(ItemCode, CodeTypingNavigationArgs.ActiveFolderEntity.Id);

                if(foundedItemId == null)
                {
                    IsAlertMessageVisible = true;
                    return;
                }
                else
                {
                    IsAlertMessageVisible = false;
                }

                var foundedItem = new ItemEntity
                {
                    Id = foundedItemId.ItemId,
                };


                await navigationService.NavigateAsync(PageNames.LoadingPopupPage, CreateParameters(new LoadingNavigationArgs()
                {
                    PageLoadingCanceled = async () =>
                    {
                        isCanceled = true;
                        await Task.CompletedTask;
                        return;
                    },
                }));

                generalItemInfoModel = await _restService.GetGeneralItemInfo(foundedItemId.ItemId);

                if (generalItemInfoModel == null)
                {
                    await navigationService.GoBackAsync();

                    return;
                }

                if (!generalItemInfoModel.IsFolder)
                {
                    langPrefs = await _baseDBService.GetAllAsync<LangPreferencesItemEntity>();
                    intersectFirstItem = generalItemInfoModel.Languages.FirstOrDefault(x => langPrefs.Any(y => x == y.LangTag));
                }

                if (!isCanceled)
                {
                    if (!string.IsNullOrEmpty(intersectFirstItem))
                    {
                        foundedItem.LangTag = intersectFirstItem;
                        foundedItem.ParentId = generalItemInfoModel.ParentId.HasValue ? generalItemInfoModel.ParentId.Value : -1;
                        foundedItem.IsFolder = generalItemInfoModel.IsFolder;
                        foundedItem.Title = generalItemInfoModel.DefaultTitle;

                        //Close Loading popup
                        await navigationService.GoBackAsync();

                        await navigationService.NavigateAsync($"../{PageNames.ItemsGalleryDetailsPage}", CreateParameters(new ItemGalleryDetailsNavigationArgs
                        {
                            ItemModel = foundedItem,
                            ItemLanguages = generalItemInfoModel.Languages,
                            NeedsToUpdatePrevious = () =>
                            {
                                CodeTypingNavigationArgs.PageUpdated.Invoke(null);
                            }
                        }));
                    }
                    else
                    {
                        //Close Loading popup
                        await navigationService.GoBackAsync();

                        if (generalItemInfoModel.Languages.Count() > 1)
                        {
                            await navigationService.NavigateAsync(PageNames.ApologizeLanguagePopupPage, CreateParameters(new ApologizeNavigationArgs
                            {
                                LanguageTags = generalItemInfoModel.Languages,
                                PopupResultAction = async (string langTagSelected) =>
                                {
                                    await UseResultToContinueNavigate(langTagSelected, foundedItem, generalItemInfoModel);
                                },
                                PageApologizeFinishedLoading = () =>
                                {
                                    IsBusy = false;
                                }
                            }));
                        }
                        else
                        {
                            await UseResultToContinueNavigate(generalItemInfoModel.Languages.FirstOrDefault(), foundedItem, generalItemInfoModel);
                        }
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

        public async Task UseResultToContinueNavigate(string langTagSelected, ItemEntity foundedItem, GeneralItemInfoModel generalItemInfoModel)
        {

            try
            {
                if (!string.IsNullOrEmpty(langTagSelected))
                {
                    foundedItem.ParentId = generalItemInfoModel.ParentId.Value;
                    foundedItem.LangTag = langTagSelected;
                    foundedItem.IsFolder = generalItemInfoModel.IsFolder;
                    foundedItem.Title = generalItemInfoModel.DefaultTitle;


                    await navigationService.NavigateAsync($"../{PageNames.ItemsGalleryDetailsPage}", CreateParameters(new ItemGalleryDetailsNavigationArgs
                    {
                        ItemModel = foundedItem,
                        ItemLanguages = generalItemInfoModel.Languages,
                        NeedsToUpdatePrevious = () =>
                        {
                            CodeTypingNavigationArgs.PageUpdated.Invoke(null);
                        }
                    }));


                    IsBusy = false;
                }
            }
            catch(Exception ex)
            {
                LogService.Log(ex);
            }
        }
    }
}
