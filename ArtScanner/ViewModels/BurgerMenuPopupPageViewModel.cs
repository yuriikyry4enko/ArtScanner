using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ArtScanner.Utils.Constants;
using ArtScanner.Views;
using Plugin.SharedTransitions;
using Prism.Navigation;
using Xamarin.Forms;
using MenuItem = ArtScanner.Models.MenuItem;

namespace ArtScanner.ViewModels
{
    public enum BurgerMenuPages
    {
        Home,
        Settings,
        ChooseLang,

    }

    class BurgerMenuPopupPageViewModel : BaseViewModel
    {
        private ObservableCollection<MenuItem> _menuItems = new ObservableCollection<MenuItem>();
        public ObservableCollection<MenuItem> MenuItems
        {
            get => _menuItems;
            set
            {
                SetProperty(ref _menuItems, value);
            }
        }

        public BurgerMenuPopupPageViewModel(
          INavigationService navigationService) : base(navigationService)
        {
            _menuItems.Add(new MenuItem()
            {
                Title = "Home",
                Page = BurgerMenuPages.Home,
            });

            _menuItems.Add(new MenuItem()
            {
                Title = "Settings",
                Page = BurgerMenuPages.Settings,
            });

            _menuItems.Add(new MenuItem()
            {
                Title = "Choose Language",
                Page = BurgerMenuPages.ChooseLang,
            });
        }

        public ICommand NavigationCommand => new Command<MenuItem>(async (item) =>
        {
            if (item.Page == BurgerMenuPages.Settings)
            {
                //await navigationService.NavigateAsync(PageNames.);
            }
            else if(item.Page == BurgerMenuPages.ChooseLang)
            {
                var actionPage = App.Current.MainPage;
                if (actionPage.Navigation != null)
                    actionPage = actionPage.Navigation.NavigationStack.Last();

                if (actionPage.GetType() != typeof(ChooseLanguagePage))
                {
                    await navigationService.NavigateAsync(PageNames.ChooseLanguagePage);
                }
            }
            else if(item.Page == BurgerMenuPages.Home)
            {
                HomePageViewModel.NeedsToUpdate = true;

                var actionPage = App.Current.MainPage;
                if (actionPage.Navigation != null)
                    actionPage = actionPage.Navigation.NavigationStack.Last();

                if (actionPage.GetType() != typeof(HomePage))
                {
                    await navigationService.NavigateAsync($"{nameof(SharedTransitionNavigationPage)}/{nameof(HomePage)}");
                }
                else
                {
                    await navigationService.GoBackAsync();
                }
            }

        });
    }

}
