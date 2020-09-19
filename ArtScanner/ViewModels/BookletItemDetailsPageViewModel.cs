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
    class BookletItemDetailsPageViewModel : BaseViewModel
    {
        private ObservableCollection<BookletItem> _bookletItems = new ObservableCollection<BookletItem>();
        public ObservableCollection<BookletItem> BookletItems
        {
            get => _bookletItems;
            set => SetProperty(ref _bookletItems, value);
        }

        public BookletItemDetailsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public ICommand ItemTappedCommad => new Command(async () =>
        {
            await navigationService.NavigateAsync(PageNames.BookleItemDetailsFolderPage);
        });

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            InitItemsList();
        }

        private void InitItemsList()
        {
            try
            {
                BookletItems.Clear();
                BookletItems.Add(new BookletItem
                {
                    Title = "First item",
                    BookletColor = Color.Orange,
                });

                BookletItems.Add(new BookletItem
                {
                    Title = "Second item",
                    BookletColor = Color.SlateGray,
                });

                BookletItems.Add(new BookletItem
                {
                    Title = "Third item",
                    BookletColor = Color.GreenYellow,
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
