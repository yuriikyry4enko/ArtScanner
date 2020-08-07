using System;
using System.Windows.Input;
using ArtScanner.Models;
using Prism.Navigation;
using Xamarin.Forms;

namespace ArtScanner.ViewModels
{
    class ItemGalleryDetailsPageViewModel : BaseViewModel
    {

        #region Properties

        private ArtModel _itemModel = new ArtModel();
        public ArtModel ItemModel
        {
            get { return _itemModel; }
            set { SetProperty(ref _itemModel, value); }
        }

        #endregion

        public ItemGalleryDetailsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.GetNavigationMode() != NavigationMode.Back)
            {
                ItemModel = GetParameters<ArtModel>(parameters);
            }
        }


        public ICommand BackCommand => new Command(async () => { await navigationService.GoBackAsync(); });

    }
}
