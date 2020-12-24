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
                //await navigationService.GoBackAsync(animated: false);
            }
        }

        private string _itemCode;
        public string ItemCode
        {
            get { return _itemCode; }
            set { SetProperty(ref _itemCode, value); }
        }

        public ICommand ContinueCommand => new Command(async () =>
        {
            try
            {
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

                var foundedItem = new ItemEntity
                {
                    Id = foundedItemId.ItemId,
                };

           
                string intersectFirstItem = string.Empty;
                List<LangPreferencesItemEntity> langPrefs;

                await navigationService.NavigateAsync(PageNames.LoadingPopupPage, CreateParameters(new LoadingNavigationArgs()
                {
                    PageLoadingCanceled = async () =>
                    {
                        isCanceled = true;
                        await Task.CompletedTask;
                        return;
                    },
                }));

                GeneralItemInfoModel result = await _restService.GetGeneralItemInfo(foundedItemId.ItemId);

                if (result == null)
                {
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
                    if (!string.IsNullOrEmpty(intersectFirstItem))
                    {
                        foundedItem.LangTag = intersectFirstItem;
                        foundedItem.ParentId = result.ParentId.HasValue ? result.ParentId.Value : -1;
                        foundedItem.IsFolder = result.IsFolder;
                        foundedItem.Title = result.DefaultTitle;

                        //Close Loading popup
                        await navigationService.GoBackAsync();

                        await navigationService.NavigateAsync($"../{PageNames.ItemsGalleryDetailsPage}", CreateParameters(new ItemGalleryDetailsNavigationArgs
                        {
                            ItemModel = foundedItem,
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


                                    await navigationService.NavigateAsync($"../{PageNames.ItemsGalleryDetailsPage}", CreateParameters(new ItemGalleryDetailsNavigationArgs
                                    {
                                        ItemModel = foundedItem,
                                        NeedsToUpdatePrevious = () =>
                                        {
                                            CodeTypingNavigationArgs.PageUpdated.Invoke(null);
                                        }
                                    }));

                                    
                                    IsBusy = false;
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
}
