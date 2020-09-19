using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using ArtScanner.Models;
using ArtScanner.Utils.Constants;
using Prism.Navigation;
using Xamarin.Forms;

namespace ArtScanner.ViewModels
{
    class BookleItemDetailsFolderPageViewModel : BaseViewModel
    {
        private ObservableCollection<BookletItem> _bookletItems = new ObservableCollection<BookletItem>();
        public ObservableCollection<BookletItem> BookletItems
        {
            get => _bookletItems;
            set => SetProperty(ref _bookletItems, value);
        }

        public BookleItemDetailsFolderPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            InitItemsList();
        }

        public ICommand ItemTappedCommad => new Command(async () =>
        {
            await navigationService.NavigateAsync(PageNames.BookleItemDetailsFolderPage);
        });

        private void InitItemsList()
        {
            try
            {
                BookletItems.Clear();
                BookletItems.Add(new BookletItem
                {
                    Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua",
                    BookletColor = Color.Violet,
                });

                BookletItems.Add(new BookletItem
                {
                    Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua",
                    BookletColor = Color.DarkOliveGreen,
                });

                BookletItems.Add(new BookletItem
                {
                    Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua",
                    BookletColor = Color.Gray,
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

    }
}
