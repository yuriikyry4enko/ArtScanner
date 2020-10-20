using System;
using System.Collections.ObjectModel;
using ArtScanner.Models;
using ArtScanner.Utils.Constants;
using Prism.Navigation;

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
                NavigationCommand = async () =>
                {
                    await navigationService.NavigateAsync(PageNames.StartPage);
                },
            });


            _menuItems.Add(new MenuItem()
            {
                Title = "Settings",
                NavigationCommand = async () =>
                {
                    await navigationService.NavigateAsync(PageNames.ChooseLanguagePage);
                },
            });

            _menuItems.Add(new MenuItem()
            {
                Title = "Choose Language",
                NavigationCommand = async () =>
                {
                    await navigationService.NavigateAsync(PageNames.ChooseLanguagePage);
                },
            });
        }
    }
}
