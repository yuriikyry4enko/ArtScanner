using System.Globalization;
using System.Linq;
using System.Threading;
using Acr.UserDialogs;
using ArtScanner.Models.Analytics;
using ArtScanner.Popups;
using ArtScanner.Resx;
using ArtScanner.Services;
using ArtScanner.ViewModels;
using ArtScanner.Views;
using Plugin.SharedTransitions;
using Prism;
using Prism.Ioc;
using Prism.Plugin.Popups;
using Prism.Unity;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace ArtScanner
{
    public partial class App : PrismApplication
    {
        public static double ScreenWidth;
        public static double ScreenHeight;

        protected override void OnStart()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CurrentCulture;

            AppResources.Culture = CultureInfo.CurrentCulture;
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

           

            var settings = (IAppSettings)Container.Resolve(typeof(IAppSettings));

            if (settings.IsLanguageSet)
            {
                var language = CultureInfo.GetCultures(CultureTypes.NeutralCultures).ToList().First(element => element.EnglishName.Contains(settings.LanguagePreferences)); ;
                AppResources.Culture = language;

                NavigationService.NavigateAsync($"{nameof(SharedTransitionNavigationPage)}/{nameof(HomePage)}");
            }
            else
            {
                NavigationService.NavigateAsync($"{nameof(SharedTransitionNavigationPage)}/{nameof(ChooseLanguagePage)}");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(UserDialogs.Instance);
            containerRegistry.RegisterInstance(PopupNavigation.Instance);
            containerRegistry.RegisterInstance(DependencyService.Get<ISQLite>());

            containerRegistry.RegisterSingleton<IAppSettings, AppSettings>();
            containerRegistry.RegisterSingleton<IAppConfig, AppConfig>();
            containerRegistry.RegisterSingleton<IAppInfo, AppInfo>();
            containerRegistry.RegisterSingleton<IAppDatabase, AppDatabase>();
            containerRegistry.RegisterSingleton<IAppFileSystemService, AppFileSystemService>();
            containerRegistry.RegisterSingleton<IRestService, RestService>();
            containerRegistry.RegisterSingleton<IBaseDBService, BaseDBService>();
            containerRegistry.RegisterSingleton<IItemDBService, ItemDBService>();
            containerRegistry.RegisterSingleton<IAnalyticsService, AnalyticsService>();

            var analyticsService = (IAnalyticsService)Container.Resolve(typeof(IAnalyticsService));
            analyticsService.SetProvider(new AnalyticsServiceFileProvider(Utils.Constants.AppConstants.csLocalAnalyticsFilePath));
            LogService.SetAnalyticsService(analyticsService);




            containerRegistry.RegisterForNavigation<ProviderLoginPage>();
            containerRegistry.RegisterForNavigation<SharedTransitionNavigationPage>();
           

            containerRegistry.RegisterForNavigation<ItemGalleryDetailsPage, ItemGalleryDetailsPageViewModel>();
            containerRegistry.RegisterForNavigation<ChooseLanguagePage, ChooseLanguagePageViewModel>();

            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<StartPage, StartPageViewModel>();
            containerRegistry.RegisterForNavigation<ScannerPage, ScannerPageViewModel>();
            containerRegistry.RegisterForNavigation<BookletItemDetailsPage, BookletItemDetailsPageViewModel>();
            containerRegistry.RegisterForNavigation<BookleItemDetailsFolderPage, BookleItemDetailsFolderPageViewModel>();
            containerRegistry.RegisterForNavigation<ItemsGalleryPage, ItemsGalleryPageViewModel>();
            containerRegistry.RegisterForNavigation<ItemGalleryDetailsPage, ItemGalleryDetailsPageViewModel>();

            containerRegistry.RegisterForNavigation<SettingsPage, SettingsPageViewModel>();

            containerRegistry.RegisterForNavigation<ApologizeLanguagePopupPage, ApologizeLanguagePopupPageViewModel>();
            containerRegistry.RegisterForNavigation<LoadingPopupPage, LoadingPopupPageViewModel>();
            containerRegistry.RegisterForNavigation<BurgerMenuPopupPage, BurgerMenuPopupPageViewModel>();

            containerRegistry.RegisterForNavigation<BurgerMenuPopupPage, BurgerMenuPopupPageViewModel>();


            //dependencyService.Get<IAnalyticsService>().SetProvider(new Framework.Data.Models.Analytics.AnalyticsServiceFileProvider(Constants.csLocalAnalyticsFilePath));
            containerRegistry.RegisterPopupNavigationService();
        }


        #endregion
    }
}
