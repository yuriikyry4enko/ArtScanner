using System;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using ArtScanner.Resx;
using ArtScanner.Services;
using ArtScanner.Utils.Constants;
using Prism.Navigation;
using Xamarin.Forms;

namespace ArtScanner.ViewModels
{
    class ChooseLanguagePageViewModel : BaseViewModel
    {
        private readonly IAppSettings appSettings;

        private string _selectedItem;
        public string SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        private bool _isWellcomeContentVisible;
        public bool IsWellcomeContentVisible
        {
            get { return _isWellcomeContentVisible; }
            set { SetProperty(ref _isWellcomeContentVisible, value); }
        }

        public ChooseLanguagePageViewModel(
            IAppSettings appSettings,
            INavigationService navigationService) : base(navigationService)
        {
            this.appSettings = appSettings;

            if (!appSettings.IsLanguageSet)
                IsWellcomeContentVisible = true;
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            if(appSettings.LanguagePreferences != string.Empty)
            {
                SelectedItem = appSettings.LanguagePreferences;
            }
        }

        public ICommand BackCommand => new Command(async () =>
        {
            await navigationService.GoBackAsync();
        });

        public ICommand OnUpdateLangugeClickedCommand => new Command(async () =>
        {
            if (SelectedItem != null)
            {
                var language = CultureInfo.GetCultures(CultureTypes.NeutralCultures).ToList().First(element => element.EnglishName.Contains(SelectedItem)); ;

                AppResources.Culture = language;

                appSettings.IsLanguageSet = true;
                appSettings.LanguagePreferences = SelectedItem;

                await navigationService.NavigateAsync(PageNames.StartPage);
            }
        });
    }
}
