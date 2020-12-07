using System;
using System.Diagnostics;
using System.Windows.Input;
using ArtScanner.Models;
using Prism.Navigation;
using Xamarin.Forms;

namespace ArtScanner.ViewModels
{
    class LoadingPopupPageViewModel : BaseViewModel
    {
        private LoadingNavigationArgs _NavArgs;

        public LoadingPopupPageViewModel(
            INavigationService navigationService) : base(navigationService)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.GetNavigationMode() != NavigationMode.Back)
            {
                _NavArgs = GetParameters<LoadingNavigationArgs>(parameters);

            }
        }

        public ICommand CloseCommand => new Command(async () =>
        {
            try
            {
                await navigationService.GoBackAsync(CreateParameters(new LoadingNavigationArgs()
                {
                    IsCanceled = false,
                })) ;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        });

        public ICommand CancelCommand => new Command(async () =>
        {
            try
            {
                await navigationService.GoBackAsync(CreateParameters(new LoadingNavigationArgs()
                {
                    IsCanceled = true,
                }));
                _NavArgs.PageLoadingCanceled.Invoke();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        });

    }
}
