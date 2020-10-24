using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ArtScanner.Models;
using ArtScanner.Models.Entities;
using ArtScanner.Services;
using ArtScanner.Utils.Constants;
using ArtScanner.Utils.Helpers;
using ArtScanner.Views;
using Plugin.SharedTransitions;
using Prism.Navigation;
using Xamarin.Forms;

namespace ArtScanner.ViewModels
{
    class BookleItemDetailsFolderPageViewModel : BaseViewModel
    {
        private readonly IUserDialogs _userDialogs;
        private readonly IItemDBService _itemDBService;
        private readonly IAppFileSystemService _appFileSystemService;
        private readonly IAppSettings appSettings;


        private ObservableCollection<ItemEntity> _bookletItems = new ObservableCollection<ItemEntity>();
        public ObservableCollection<ItemEntity> BookletItems
        {
            get => _bookletItems;
            set => SetProperty(ref _bookletItems, value);
        }

        private CategoryItemEntity _navigatedItem;
        public CategoryItemEntity NavigatedItem
        {
            get => _navigatedItem;
            set => SetProperty(ref _navigatedItem, value);
        }


        private ItemEntity _selectedItem;
        public ItemEntity SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public BookleItemDetailsFolderPageViewModel
            (IUserDialogs userDialogs,
            INavigationService navigationService,
            IAppFileSystemService appFileSystemService,
            IAppSettings appSettings,
            IItemDBService itemDBService) : base(navigationService)
        {
            this._userDialogs = userDialogs;
            this._itemDBService = itemDBService;
            this._appFileSystemService = appFileSystemService;
            this.appSettings = appSettings;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.GetNavigationMode() != NavigationMode.Back)
            {
                NavigatedItem = GetParameters<CategoryItemEntity>(parameters);

                await InitItemsList();
            }
        }

        public ICommand ItemTappedCommad => new Command<ItemEntity>(async (item) =>
        {
            await navigationService.NavigateAsync(PageNames.ItemsGalleryDetailsPage, CreateParameters(item));
        });

        public ICommand DeleteCommand => new Command(async (item) =>
        {
            var result = await _userDialogs.ConfirmAsync("Remove item?", "Confirmation", "Yes", "No");

            if (!result) return;

            var model = item as ItemEntity;

            await _itemDBService.DeleateItem(model);

            BookletItems.Remove(model);

            if (BookletItems.Count == 0)
            {
                await _itemDBService.DeleateCategoryItem(NavigatedItem);
                await _itemDBService.CheckAndDeleteFolder(NavigatedItem.ParentId, NavigatedItem.LocalId);

                appSettings.NeedToUpdateHomePage = true;

                await navigationService.GoBackToRootAsync();
                await navigationService.NavigateAsync($"{nameof(HomePage)}");
            }
        });

        private async Task InitItemsList()
        {
            try
            {
                BookletItems.Clear();

                var result = await _itemDBService.GetItemsByParentIdAll(NavigatedItem.Id);
                foreach(var item in result)
                {
                    BookletItems.Add(new ItemEntity
                    {
                        Liked = item.Liked,
                        LangTag = item.LangTag,
                        Author = item.Author,
                        LocalId = item.LocalId,
                        MusicUrl = item.MusicUrl,
                        ImageFileName = item.ImageFileName,
                        ImageUrl = item.ImageUrl,
                        ParentId = item.ParentId,
                        MusicFileName = item.MusicFileName,
                        Description = item.Description,
                        Id = item.Id,
                        Title = item.Title,
                        ImageByteArray = StreamHelpers.GetByteArrayFromFilePath(_appFileSystemService.GetFilePath(item.ImageFileName)),
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
