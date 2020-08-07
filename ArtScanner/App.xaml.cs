using Acr.UserDialogs;
using ArtScanner.Services;
using ArtScanner.Utils.Constants;
using ArtScanner.ViewModels;
using ArtScanner.Views;
using Plugin.SharedTransitions;
using Prism;
using Prism.Ioc;
using Prism.Unity;

namespace ArtScanner
{
    public partial class App : PrismApplication
    {
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        #region Prism

        public App(IPlatformInitializer platformInitializer = null) : base(platformInitializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();
            //NavigationService.NavigateAsync(PageNames.ItemsGalleryPage);

            NavigationService.NavigateAsync($"{nameof(SharedTransitionNavigationPage)}/{nameof(StartPage)}");


        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            containerRegistry.RegisterInstance(UserDialogs.Instance);

            containerRegistry.RegisterSingleton<SocialService>();
            containerRegistry.RegisterSingleton<ITwitterService, TwitterService>();

            containerRegistry.RegisterForNavigation<ProviderLoginPage>();
            containerRegistry.RegisterForNavigation<SharedTransitionNavigationPage>();
            containerRegistry.RegisterForNavigation<StartPage, StartPageViewModel>();
            containerRegistry.RegisterForNavigation<ScannerPage, ScannerPageViewModel>();
            containerRegistry.RegisterForNavigation<ArtDetailsPage, ArtDetailsPageViewModel>();
            containerRegistry.RegisterForNavigation<ItemsGalleryPage, ItemsGalleryPageViewModel>();
            containerRegistry.RegisterForNavigation<ItemGalleryDetailsPage, ItemGalleryDetailsPageViewModel>();

        }


        #endregion
    }
}
