using Acr.UserDialogs;
using ArtScanner.Utils.Constants;
using ArtScanner.ViewModels;
using ArtScanner.Views;
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
            NavigationService.NavigateAsync(PageNames.StartPage);


        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            containerRegistry.RegisterInstance(UserDialogs.Instance);

            containerRegistry.RegisterForNavigation<StartPage, StartPageViewModel>();
            containerRegistry.RegisterForNavigation<ScannerPage, ScannerPageViewModel>();
            containerRegistry.RegisterForNavigation<ArtDetailsPage, ArtDetailsPageViewModel>();

        }


        #endregion
    }
}
