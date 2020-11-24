using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ArtScanner.Models;
using ArtScanner.Models.Entities;
using ArtScanner.Services;
using ArtScanner.Utils.Constants;
using ArtScanner.Utils.Helpers;
using ArtScanner.Views;
using Prism.Navigation;
using Xamarin.Forms;

namespace ArtScanner.ViewModels
{
    class BookletItemDetailsPageViewModel : BaseViewModel
    {
        private readonly IUserDialogs _userDialogs;
        private readonly IItemDBService _itemDBService;
        private readonly IAppFileSystemService _appFileSystemService;
        private readonly IAppSettings appSettings;
        private readonly IRestService _restService;

        private ObservableCollection<ItemEntity> _bookletItems = new ObservableCollection<ItemEntity>();
        public ObservableCollection<ItemEntity> BookletItems
        {
            get => _bookletItems;
            set => SetProperty(ref _bookletItems, value);
        }

        private ItemEntity _selectedBookletItem;
        public ItemEntity SelectedBookletItem
        {
            get => _selectedBookletItem;
            set => SetProperty(ref _selectedBookletItem, value);
        }

        private ItemEntity _navigatedFolderItem;
        public ItemEntity NavigatedFolderItem
        {
            get => _navigatedFolderItem;
            set => SetProperty(ref _navigatedFolderItem, value);
        }

        public BookletItemDetailsPageViewModel(
            IUserDialogs userDialogs,
            INavigationService navigationService,
            IAppFileSystemService appFileSystemService,
            IAppSettings appSettings,
            IRestService restService,
            IItemDBService itemDBService) : base(navigationService)
        {
            this._userDialogs = userDialogs;
            this._itemDBService = itemDBService;
            this._appFileSystemService = appFileSystemService;
            this.appSettings = appSettings;
            this._restService = restService;
        }

        public ICommand ItemTappedCommad => new Command<ItemEntity>(async (item) =>
        {
            if (!item.IsFolder)
            {
                await navigationService.NavigateAsync(PageNames.ItemsGalleryPage, CreateParameters(new GalleryNavigationArgs
                {
                    SelectedChildModel = item,
                    NavigatedModel = NavigatedFolderItem,
                }));
            }
            else
            {
                await navigationService.NavigateAsync(PageNames.BookletItemDetailsPage, CreateParameters(item));
            }
        });

        public ICommand DeleteCommand => new Command(async (item) =>
        {
            var model = item as ItemEntity;

            int countOfCategoryItems = (await _itemDBService.GetItemsByParentIdAll(model.Id)).Count();
            var result = await _userDialogs.ConfirmAsync($"Remove {countOfCategoryItems} items?", "Confirmation", "Yes", "No");

            if (!result) return;

            await _itemDBService.DeleateItemWithChildren(model);

            BookletItems.Remove(model);

            appSettings.NeedToUpdateHomePage = true;

            if (BookletItems.Count == 0)
            {
                await _itemDBService.DeleateItem(NavigatedFolderItem);

                await navigationService.GoBackToRootAsync();
                await navigationService.NavigateAsync($"{nameof(HomePage)}");
            }
        });

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.GetNavigationMode() != NavigationMode.Back)
            {
                NavigatedFolderItem = GetParameters<ItemEntity>(parameters);

                if(NavigatedFolderItem?.ImageByteArray == null && NavigatedFolderItem.Id != 0)
                {
                    NavigatedFolderItem.ImageByteArray = await _restService.GetImageById(NavigatedFolderItem.Id);
                    RaisePropertyChanged(nameof(NavigatedFolderItem));

                }

                await InitItemsList();
            }
        }

        private async Task InitItemsList()
        {
            try
            {
                BookletItems.Clear();

                var result = await _itemDBService.GetItemsByParentIdAll(NavigatedFolderItem.Id);
                foreach (var item in result)
                {
                    BookletItems.Add(new ItemEntity
                    {
                        LocalId = item.LocalId,
                        Id = item.Id,
                        Description = item.Description,
                        ParentId = item.ParentId,
                        Title = item.Title,
                        IsFolder = item.IsFolder,
                        ImageByteArray = StreamHelpers.GetByteArrayFromFilePath(_appFileSystemService.GetFilePath(item.ImageFileName))
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
