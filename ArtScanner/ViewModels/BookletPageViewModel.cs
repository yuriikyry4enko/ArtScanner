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
    class BookletPageViewModel : BaseViewModel
    {
        private ObservableCollection<BookletItem> _bookletItems = new ObservableCollection<BookletItem>();
        public ObservableCollection<BookletItem> BookletItems
        {
            get => _bookletItems;
            set => SetProperty(ref _bookletItems, value);
        }

        #region Ctor

        public BookletPageViewModel(
            INavigationService navigationService) : base(navigationService)
        {
        }

        #endregion

        public ICommand ItemTappedCommad => new Command(async () =>
        {
            await navigationService.NavigateAsync(PageNames.BookletItemDetailsPage);
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
                    Title = "Arts",
                    BookletColor = Color.Aquamarine,
                });

                BookletItems.Add(new BookletItem
                {
                    Title = "Zoos",
                    BookletColor = Color.IndianRed,
                });
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }


    }
}
