using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using ArtScanner.Models.Entities;
using ArtScanner.Services;
using ArtScanner.Utils.Constants;
using ArtScanner.Utils.Helpers;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace ArtScanner.ViewModels
{
    class ItemsGalleryPageViewModel : BaseViewModel
    {
        #region Services

        private readonly IAppFileSystemService _appFileSystemService;
        private readonly IItemDBService _itemDBService;

        #endregion

        #region Properties

        private ObservableCollection<ItemEntity> _items = new ObservableCollection<ItemEntity>();
        public ObservableCollection<ItemEntity> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }
        
        private string _selectedId;
        public string SelectedId
        {
            get => _selectedId;
            set => SetProperty(ref _selectedId, value);
        }

        #endregion 

        #region Ctor

        public ItemsGalleryPageViewModel
            (INavigationService navigationService,
            IItemDBService itemDBService,
            IAppFileSystemService appFileSystemService) : base(navigationService)
        {

            this._itemDBService = itemDBService;
            this._appFileSystemService = appFileSystemService;
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                Items.Clear();

                var items = await _itemDBService.GetAll();
                if (items.Count > 0)
                {
                    Items = new ObservableCollection<ItemEntity>(items);

                    foreach (var item in Items)
                    {
                        item.ImageByteArray = StreamHelpers.GetByteArrayFromFilePath(_appFileSystemService.GetFilePath(item.ImageFileName));
                    }

                    RaisePropertyChanged(nameof(Items));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }


        #endregion

        #region Commands

        public ICommand BackCommand => new Command(async () => { await navigationService.GoBackAsync(); });

        public ICommand NavigateToItemDetail => new DelegateCommand<ItemEntity>(async(itemModel) =>
        {
            SelectedId = itemModel.LocalId.ToString();

            await navigationService.NavigateAsync(PageNames.ItemsGalleryDetailsPage, CreateParameters(itemModel));
        });

        #endregion

    }
}
