using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ArtScanner.Services;
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
        SendLogs,
    }

    class BurgerMenuPopupPageViewModel : BaseViewModel
    {
        private readonly IFileService _fileService;

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
            IFileService fileService,
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
            _menuItems.Add(new MenuItem()
            {
                Title = "Send Logs",
                Page = BurgerMenuPages.SendLogs,
            });
        }

        public ICommand NavigationCommand => new Command<MenuItem>(async (item) =>
        {
            try
            {
                var actionPage = App.Current.MainPage;

                switch (item.Page)
                {
                    case BurgerMenuPages.Settings:

                        break;

                    case BurgerMenuPages.ChooseLang:
                       
                        if (actionPage.Navigation != null)
                            actionPage = actionPage.Navigation.NavigationStack.Last();

                        if (actionPage.GetType() != typeof(ChooseLanguagePage))
                        {
                            await navigationService.NavigateAsync(PageNames.ChooseLanguagePage);
                        }
                        break;

                    case BurgerMenuPages.Home:

                        HomePageViewModel.NeedsToUpdate = true;

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
                        break;

                    case BurgerMenuPages.SendLogs:
                        Xamarin.Essentials.EmailMessage emailMessage = new Xamarin.Essentials.EmailMessage();
                        emailMessage.To = new System.Collections.Generic.List<string>() { Utils.Constants.AppConstants.csEmailSupport };
                        emailMessage.Subject = Utils.Constants.AppConstants.csEmailSupportSubject;
                        emailMessage.Body = Utils.Constants.AppConstants.csEmailSupportBody;
                        emailMessage.Attachments.Add(new Xamarin.Essentials.EmailAttachment(_fileService.GetAbsolutPath("logs.txt")));
                        await Xamarin.Essentials.Email.ComposeAsync(emailMessage);
                      
                        break;
                }
            }
            catch(Exception ex)
            {
                LogService.Log(ex);
            }
            //if (item.Page == BurgerMenuPages.Settings)
            //{
            //    //await navigationService.NavigateAsync(PageNames.);
            //}
            //else if(item.Page == BurgerMenuPages.ChooseLang)
            //{
            //    var actionPage = App.Current.MainPage;
            //    if (actionPage.Navigation != null)
            //        actionPage = actionPage.Navigation.NavigationStack.Last();

            //    if (actionPage.GetType() != typeof(ChooseLanguagePage))
            //    {
            //        await navigationService.NavigateAsync(PageNames.ChooseLanguagePage);
            //    }
            //}
            //else if(item.Page == BurgerMenuPages.Home)
            //{
            //    HomePageViewModel.NeedsToUpdate = true;

            //    var actionPage = App.Current.MainPage;
            //    if (actionPage.Navigation != null)
            //        actionPage = actionPage.Navigation.NavigationStack.Last();

            //    if (actionPage.GetType() != typeof(HomePage))
            //    {
            //        await navigationService.NavigateAsync($"{nameof(SharedTransitionNavigationPage)}/{nameof(HomePage)}");
            //    }
            //    else
            //    {
            //        await navigationService.GoBackAsync();
            //    }
            //}
            //else if(item.Page == BurgerMenuPages.SendLogs)
            //{
            //    try
            //    {
            //        Xamarin.Essentials.EmailMessage emailMessage = new Xamarin.Essentials.EmailMessage();
            //        emailMessage.To = new System.Collections.Generic.List<string>() { Utils.Constants.AppConstants.csEmailSupport };
            //        emailMessage.Subject = Utils.Constants.AppConstants.csEmailSupportSubject;
            //        emailMessage.Body = Utils.Constants.AppConstants.csEmailSupportBody;
            //        emailMessage.Attachments.Add(new Xamarin.Essentials.EmailAttachment(Utils.Constants.AppConstants.csLocalAnalyticsFilePath));
            //        await Xamarin.Essentials.Email.ComposeAsync(emailMessage);
            //    }
            //    catch(Exception ex)
            //    {
            //        LogService.Log(ex); 
            //    }
            //}

        });
    }

}
