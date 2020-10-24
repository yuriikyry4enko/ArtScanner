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
using Plugin.SharedTransitions;
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

        private ObservableCollection<CategoryItemEntity> _bookletItems = new ObservableCollection<CategoryItemEntity>();
        public ObservableCollection<CategoryItemEntity> BookletItems
        {
            get => _bookletItems;
            set => SetProperty(ref _bookletItems, value);
        }

        private CategoryItemEntity _selectedBookletItem;
        public CategoryItemEntity SelectedBookletItem
        {
            get => _selectedBookletItem;
            set => SetProperty(ref _selectedBookletItem, value);
        }

        private FolderItemEntity _navigatedFolderItem;
        public FolderItemEntity NavigatedFolderItem
        {
            get => _navigatedFolderItem;
            set => SetProperty(ref _navigatedFolderItem, value);
        }

        public BookletItemDetailsPageViewModel(
            IUserDialogs userDialogs,
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

        public ICommand ItemTappedCommad => new Command<CategoryItemEntity>(async (item) =>
        {
            await navigationService.NavigateAsync(PageNames.BookleItemDetailsFolderPage, CreateParameters(item));
        });

        public ICommand DeleteCommand => new Command(async (item) =>
        {
            var model = item as CategoryItemEntity;

            int countOfCategoryItems = (await _itemDBService.GetItemsByParentIdAll(model.Id)).Count();
            var result = await _userDialogs.ConfirmAsync($"Remove category with {countOfCategoryItems} items?", "Confirmation", "Yes", "No");

            if (!result) return;

           
            await _itemDBService.DeleateCategoryItem(model);

            BookletItems.Remove(model);

            appSettings.NeedToUpdateHomePage = true;

            if (BookletItems.Count == 0)
            {

                await _itemDBService.DeleateFolderItem(NavigatedFolderItem);

                await navigationService.GoBackToRootAsync();
                await navigationService.NavigateAsync($"{nameof(HomePage)}");
            }
        });

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.GetNavigationMode() != NavigationMode.Back)
            {
                NavigatedFolderItem = GetParameters<FolderItemEntity>(parameters);

                await InitItemsList();
            }
        }

        private async Task InitItemsList()
        {
            try
            {
                BookletItems.Clear();

                var result = await _itemDBService.GetCategoriesByParentIdAll(NavigatedFolderItem.Id);
                foreach (var item in result)
                {
                    BookletItems.Add(new CategoryItemEntity
                    {
                        LocalId = item.LocalId,
                        Id = item.Id,
                        ParentId = item.ParentId,
                        Title = item.Title,
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
