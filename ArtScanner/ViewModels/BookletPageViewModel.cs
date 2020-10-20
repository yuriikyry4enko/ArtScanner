using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ArtScanner.Models.Entities;
using ArtScanner.Services;
using ArtScanner.Utils.Constants;
using ArtScanner.Utils.Helpers;
using Prism.Navigation;
using Xamarin.Forms;

namespace ArtScanner.ViewModels
{
    class BookletPageViewModel : BaseViewModel
    {
        private readonly IUserDialogs _userDialogs;
        private readonly IItemDBService _itemDBService;
        private readonly IAppFileSystemService _appFileSystemService;

        private ObservableCollection<FolderItemEntity> _bookletItems = new ObservableCollection<FolderItemEntity>();
        public ObservableCollection<FolderItemEntity> BookletItems
        {
            get => _bookletItems;
            set => SetProperty(ref _bookletItems, value);
        }

        private FolderItemEntity _selectedBookletItem;
        public FolderItemEntity SelectedBookletItem
        {
            get => _selectedBookletItem;
            set => SetProperty(ref _selectedBookletItem, value);
        }

        #region Ctor

        public BookletPageViewModel(
            INavigationService navigationService,
            IUserDialogs userDialogs, 
            IAppFileSystemService appFileSystemService,
            IItemDBService itemDBService) : base(navigationService)
        {
            this._itemDBService = itemDBService;
            this._appFileSystemService = appFileSystemService;
            this._userDialogs = userDialogs;
        }

        #endregion

        public ICommand ItemTappedCommad => new Command<FolderItemEntity>(async (item) =>
        {
            await navigationService.NavigateAsync(PageNames.BookletItemDetailsPage, CreateParameters(item));
        });

        public ICommand DeleteCommand => new Command(async (item) =>
        {
            var model = item as FolderItemEntity;

            var folderCategoriesCount = (await _itemDBService.GetCategoriesByParentIdAll(model.Id)).Count();
            var result = await _userDialogs.ConfirmAsync($"Remove folder with {folderCategoriesCount} categories?", "Confirmation", "Yes", "No");

            if (!result) return;

            await _itemDBService.DeleateFolderItem(model);

            BookletItems.Remove(model);

            if(BookletItems.Count == 0)
            {
                await navigationService.GoBackAsync();
            }
        });

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.GetNavigationMode() != NavigationMode.Back)
            { 
                await InitItemsList();
            }
        }

        private async Task InitItemsList()
        {
            try
            {
                BookletItems.Clear();

                var result = await _itemDBService.GetAllFolders();
                foreach (var item in result)
                {
                    BookletItems.Add(new FolderItemEntity
                    {
                        LocalId = item.LocalId,
                        Id = item.Id,
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
