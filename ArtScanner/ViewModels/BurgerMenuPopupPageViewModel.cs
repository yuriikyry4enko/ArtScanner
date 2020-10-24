using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ArtScanner.Models;
using ArtScanner.Utils.Constants;
using ArtScanner.Views;
using Plugin.SharedTransitions;
using Prism.Navigation;
using Xamarin.Forms;
using MenuItem = ArtScanner.Models.MenuItem;

namespace ArtScanner.ViewModels
{
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
            });


            _menuItems.Add(new MenuItem()
            {
                Title = "Settings",
            });

            _menuItems.Add(new MenuItem()
            {
                Title = "Choose Language",
            });

            _menuItems.Add(new MenuItem()
            {
                Title = "Galley",
            });
        }

        public ICommand NavigationCommand => new Command<MenuItem>(async (item) =>
        {
            if (item.Title == "Settings")
            {
                //await navigationService.NavigateAsync(PageNames.);
            }
            else if(item.Title == "Choose Language")
            {
                await navigationService.NavigateAsync(PageNames.ChooseLanguagePage);
            }
            else if(item.Title == "Home")
            {
                await navigationService.NavigateAsync($"{nameof(SharedTransitionNavigationPage)}/{nameof(HomePage)}");
            }
            else if(item.Title == "Galley")
            {
                await navigationService.NavigateAsync(PageNames.ItemsGalleryPage);
            }

        });
    }

}
