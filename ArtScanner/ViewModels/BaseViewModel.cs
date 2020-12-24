using System;
using System.Windows.Input;
using ArtScanner.Utils.Constants;
using Prism.AppModel;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace ArtScanner.ViewModels
{
    class BaseViewModel : BindableBase, IInitialize, INavigationAware, IDestructible, IPageLifecycleAware
    {
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public virtual ICommand BackCommand => new Command(async () => { await navigationService.GoBackAsync(); });

        public ICommand OpenBurgerMenuCommand => new Command(async () =>
        {
            await navigationService.NavigateAsync(PageNames.BurgerMenuPopupPage);
        });

        protected const string Params = "params";

        protected readonly INavigationService navigationService;

        public INavigationParameters CreateParameters(object obj)
        {
            var parameters = new NavigationParameters();
            parameters.Add(Params, obj);
            return parameters;
        }

        protected T GetParameters<T>(INavigationParameters parameters) where T : class
        {
            return parameters[Params] as T;
        }

        protected object GetParameters(INavigationParameters parameters)
        {
            return parameters[Params];
        }

        protected bool HasParameters(INavigationParameters parameters)
        {
            return parameters.ContainsKey(Params);
        }

        public BaseViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }

        public virtual void OnAppearing()
        {

        }

        public virtual void OnDisappearing()
        {

        }
    }
}
