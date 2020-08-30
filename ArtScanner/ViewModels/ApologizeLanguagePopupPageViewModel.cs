using System;
using System.Windows.Input;
using ArtScanner.Services;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace ArtScanner.ViewModels
{
    class ApologizeLanguagePopupPageViewModel : BaseViewModel
    {

        public ApologizeLanguagePopupPageViewModel(
            INavigationService navigationService) : base(navigationService)
        {
        }

        public ICommand CloseCommand => new Command(async () =>
        {
            await navigationService.GoBackAsync(CreateParameters(new object { }));
        });

    }
}
