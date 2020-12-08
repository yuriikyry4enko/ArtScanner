using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using ArtScanner.Models;
using ArtScanner.Models.Entities;
using ArtScanner.Services;
using Prism.Navigation;
using Xamarin.Forms;

namespace ArtScanner.ViewModels
{
    class ApologizeLanguagePopupPageViewModel : BaseViewModel
    {
        private readonly IBaseDBService _baseDBService;

        private ApologizeNavigationArgs _NavArgs { get; set; }

        private ObservableCollection<CultureInfoModelExtended> _availableCulturesList = new ObservableCollection<CultureInfoModelExtended>();
        public ObservableCollection<CultureInfoModelExtended> AvailableCulturesList
        {
            get => _availableCulturesList;
            set
            {
                SetProperty(ref _availableCulturesList, value);
            }
        }

        private CultureInfoModelExtended _selectedCulture;
        public CultureInfoModelExtended SelectedCulture
        {
            get => _selectedCulture;
            set
            {
                SetProperty(ref _selectedCulture, value);
            }
        }

        private bool _isSavePreference = true;
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

            if (parameters.GetNavigationMode() != NavigationMode.Back)
            {
                _NavArgs = GetParameters<ApologizeNavigationArgs>(parameters);

                try
                {
                    foreach (var item in _NavArgs.LanguageTags)
                    {
                        AvailableCulturesList.Add(new CultureInfoModelExtended(item));
                    }
                }
                catch (Exception ex)
                {
                    LogService.Log(ex);
                }
                finally
                {
                    _NavArgs.PageApologizeFinishedLoading.Invoke();
                }
            }
        }

        public ICommand CloseCommand => new Command(async () =>
        {
            try
            {
                await navigationService.GoBackAsync();
                _NavArgs.PopupResultAction.Invoke(null);
            }
            catch(Exception ex)
            {
                LogService.Log(ex);
            }
        });

        public ICommand OkCommand => new Command(async () =>
        {
            try
            {
                if (IsSavePreference && SelectedCulture != null)
                {
                    var item = new LangPreferencesItemEntity()
                    {
                        LangTag = SelectedCulture.TwoLetterISOLanguageName,
                        NativeName = SelectedCulture.NativeName,

                    };

                    await _baseDBService.Add(item);
                }

                await navigationService.GoBackAsync();
                _NavArgs.PopupResultAction.Invoke(SelectedCulture?.TwoLetterISOLanguageName);
            }
            catch(Exception ex)
            {
                LogService.Log(ex);
            }
        });

        public ICommand CheckedChangedCommand => new Command<CultureInfoModelExtended>((checkedObject) =>
        {
            try
            {
                if (checkedObject != null)
                {
                    AvailableCulturesList.Where(x => x != checkedObject).All(y => y.IsChecked = false);
                    SelectedCulture = checkedObject;
                }
            }
            catch (Exception ex)
            {
                LogService.Log(ex);
            }
        });

    }
}
