using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using ArtScanner.Models;
using ArtScanner.Models.Entities;
using ArtScanner.Services;
using ArtScanner.Utils.Constants;
using ArtScanner.Utils.Helpers;
using Prism.Commands;
using Prism.Navigation;

namespace ArtScanner.ViewModels
{
    class ItemsGalleryPageViewModel : BaseViewModel
    {
        #region Services

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
                    foreach (var item in items)
                    {
                        Items.Add(new ItemEntityViewModel()
                        {
                            ImageByteArray = StreamHelpers.GetByteArrayFromFilePath(_appFileSystemService.GetFilePath(item.ImageFileName)),
                            Author = item.Author,
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
                            WikiUrl = item.WikiUrl,
                        });
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

        public ICommand NavigateToItemDetail => new DelegateCommand<ItemEntityViewModel>(async(itemModel) =>
        {
            SelectedId = itemModel.LocalId.ToString();

            await navigationService.NavigateAsync(PageNames.ItemsGalleryDetailsPage, CreateParameters(itemModel));
        });

        public ICommand CarouselGalletyItemChangedCommand => new DelegateCommand<ItemEntityViewModel>(async (itemModel) =>
        {
           
        });

        #endregion

    }
}
