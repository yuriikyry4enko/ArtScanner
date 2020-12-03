using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using ArtScanner.Models;
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

        private GalleryNavigationArgs _navArgs;
        private readonly IAppFileSystemService _appFileSystemService;
        private readonly IItemDBService _itemDBService;

        #endregion

        #region Properties

        private ObservableCollection<ItemEntityViewModel> _items = new ObservableCollection<ItemEntityViewModel>();
        public ObservableCollection<ItemEntityViewModel> Items
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

        private ItemEntityViewModel _currentCarouselItem = new ItemEntityViewModel();
        public ItemEntityViewModel CurrentCarouselItem
        {
            get => _currentCarouselItem;
            set => SetProperty(ref _currentCarouselItem, value);
        }

        private ItemEntity _navigatedItemModel = new ItemEntity();
        public ItemEntity NavigatedItemModel
        {
            get { return _navigatedItemModel; }
            set
            {
                SetProperty(ref _navigatedItemModel, value);
            }
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

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.GetNavigationMode() != NavigationMode.Back)
            {
                try
                {
                    _navArgs = GetParameters<GalleryNavigationArgs>(parameters);
                    NavigatedItemModel = _navArgs.NavigatedModel;


                    Items.Clear();

                    var items = await _itemDBService.GetItemsByParentIdAll(NavigatedItemModel.Id);

                    if (items.Count() > 0)
                    {
                        foreach (var item in items)
                        {
                            Items.Add(new ItemEntityViewModel()
                            {
                                ImageByteArray = StreamHelpers.GetByteArrayFromFilePath(_appFileSystemService.GetFilePath(item.ImageFileName)),
                                MusicByteArray = item.MusicByteArray,
                                Id = item.Id,
                                Description = item.Description,
                                ImageFileName = item.ImageFileName,
                                ImageUrl = item.ImageUrl,
                                LangTag = item.LangTag,
                                Liked = item.Liked,
                                LocalId = item.LocalId,
                                MusicFileName = item.MusicFileName,
                                MusicUrl = item.MusicUrl,
                                ParentId = item.ParentId,
                                Title = item.Title,
                            });
                        }

                        CurrentCarouselItem = Items.FirstOrDefault(x => x.Id == _navArgs.SelectedChildModel.Id);

                        RaisePropertyChanged(nameof(Items));
                        RaisePropertyChanged(nameof(CurrentCarouselItem));

                    }
                }
                catch (Exception ex)
                {
                    LogService.Log(ex);
                }
            }
        }

        #endregion

        #region Commands

        public ICommand NavigateToItemDetail => new DelegateCommand<ItemEntityViewModel>(async(itemModel) =>
        {
            SelectedId = itemModel.LocalId.ToString();

            await navigationService.NavigateAsync(PageNames.ItemsGalleryDetailsPage, CreateParameters(itemModel));
        });

        public ICommand TapItemCarouselSmallImagesCommand => new Command<ItemEntityViewModel>((itemModel) =>
        {
            CurrentCarouselItem = itemModel;
        });

        #endregion

    }
}
