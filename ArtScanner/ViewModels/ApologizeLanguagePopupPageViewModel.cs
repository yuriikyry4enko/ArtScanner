using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Input;
using ArtScanner.Models;
using ArtScanner.Models.Entities;
using ArtScanner.Services;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace ArtScanner.ViewModels
{
    class ApologizeLanguagePopupPageViewModel : BaseViewModel
    {
        private readonly IBaseDBService _baseDBService;

        private ApologizeNavigationArgs _NavArgs { get; set; }

        private ObservableCollection<CultureInfo> _availableCulturesList = new ObservableCollection<CultureInfo>();
        public ObservableCollection<CultureInfo> AvailableCulturesList
        {
            get => _availableCulturesList;
            set
            {
                SetProperty(ref _availableCulturesList, value);
            }
        }

        private CultureInfo _selectedCulture;
        public CultureInfo SelectedCulture
        {
            get => _selectedCulture;
            set
            {
                SetProperty(ref _selectedCulture, value);
            }
        }

        private bool _isSavePreference;
        public bool IsSavePreference
        {
            get => _isSavePreference;
            set
            {
                SetProperty(ref _isSavePreference, value);
            }
        }

        public ApologizeLanguagePopupPageViewModel(
            INavigationService navigationService,
            IBaseDBService baseDBService) : base(navigationService)
        {
            this._baseDBService = baseDBService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _NavArgs = GetParameters<ApologizeNavigationArgs>(parameters);

            try
            {
                foreach (var item in _NavArgs.LanguageTags)
                {
                    AvailableCulturesList.Add(new CultureInfo(item));
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                _NavArgs.PageApologizeFinishedLoading.Invoke();
            }
        }

        public ICommand CloseCommand => new Command(async () =>
        {
            await navigationService.GoBackAsync();
            _NavArgs.PopupResultAction.Invoke(null);
        });

        public ICommand OkCommand => new Command(async () =>
        {
            if(IsSavePreference && SelectedCulture != null)
            {
                var item = new LangPreferencesItemEntity()
                {
                    LangTag = SelectedCulture.TwoLetterISOLanguageName,
                    NativeName = SelectedCulture.NativeName,
                    
                };

                await _baseDBService.Add(item);
            }

            await navigationService.GoBackAsync();
            _NavArgs.PopupResultAction.Invoke(SelectedCulture.TwoLetterISOLanguageName);
        });



    }
}
