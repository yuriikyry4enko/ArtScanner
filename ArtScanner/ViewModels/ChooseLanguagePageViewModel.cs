using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using ArtScanner.Models.Entities;
using ArtScanner.Resx;
using ArtScanner.Services;
using ArtScanner.Utils.Constants;
using ArtScanner.Utils.Helpers;
using ArtScanner.Views;
using Plugin.SharedTransitions;
using Prism.Navigation;
using Xamarin.Forms;

namespace ArtScanner.ViewModels
{
    class ChooseLanguagePageViewModel : BaseViewModel
    {
        private readonly IAppSettings appSettings;
        private readonly IUserDialogs userDialogs;
        private readonly IBaseDBService baseDBService;
        private readonly IAppDatabase _appDatabase;
        private readonly IAppFileSystemService _appFileSystemService;

        private List<LangPreferencesItemEntity> ItemsLangForDelete = new List<LangPreferencesItemEntity>();

        private ObservableCollection<LangPreferencesItemEntity> _languagePreferencesList = new ObservableCollection<LangPreferencesItemEntity>();
        public ObservableCollection<LangPreferencesItemEntity> LanguagePreferencesList
        {
            get => _languagePreferencesList;
            set => SetProperty(ref _languagePreferencesList, value);
        }

        private ObservableCollection<CultureInfo> _culturesList = new ObservableCollection<CultureInfo>();
        public ObservableCollection<CultureInfo> CulturesList
        {
            get => _culturesList;
            set
            {
                SetProperty(ref _culturesList, value);
                RaisePropertyChanged(nameof(IsRemoveItemButtonVisible));
            }
        }

        private bool _isWellcomeContentVisible;
        public bool IsWellcomeContentVisible
        {
            get { return _isWellcomeContentVisible; }
            set { SetProperty(ref _isWellcomeContentVisible, value); }
        }

        private bool _isRemoveItemButtonVisible;
        public bool IsRemoveItemButtonVisible
        {
            get { return LanguagePreferencesList?.Count > 1; }
            set { SetProperty(ref _isRemoveItemButtonVisible, value); }
        }

        public ChooseLanguagePageViewModel(
            IAppSettings appSettings,
            IUserDialogs userDialogs,
            IAppDatabase appDatabase,
            IAppFileSystemService appFileSystemService,
            IBaseDBService baseDBService,
            INavigationService navigationService) : base(navigationService)
        {
            this.baseDBService = baseDBService;

            this.userDialogs = userDialogs;
            this.appSettings = appSettings;
            this._appDatabase = appDatabase;
            this._appFileSystemService = appFileSystemService;


            if (!appSettings.IsLanguageSet)
                IsWellcomeContentVisible = true;

            CulturesList = new ObservableCollection<CultureInfo>(CultureInfo.GetCultures(CultureTypes.AllCultures)
                         .Except(CultureInfo.GetCultures(CultureTypes.SpecificCultures)));

            CulturesList?.Remove(CulturesList[0]);
            CulturesList.Remove(CultureInfo.CurrentCulture);
            CulturesList.Insert(0, CultureInfo.CurrentCulture);
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                LanguagePreferencesList.Clear();
                ItemsLangForDelete.Clear();


                if (!appSettings.IsLanguageSet)
                {
                    if (LanguagePreferencesList.Count == 0)
                    {
                        var item = new LangPreferencesItemEntity()
                        {
                            SelectedCultureIndex = 0,
                            NativeName = CultureInfo.CurrentCulture.NativeName,
                            LangTag = CultureInfo.CurrentCulture.TwoLetterISOLanguageName,
                            SelectedCulture = CultureInfo.CurrentCulture,
                        };

                        LanguagePreferencesList.Add(item);
                    }
                }
                else
                {
                    var items = await baseDBService.GetAllAsync<LangPreferencesItemEntity>();

                    foreach (var item in items)
                    {
                        item.SelectedCulture = item.SelectedCultureIndex > 0 ? CulturesList.ElementAt(item.SelectedCultureIndex) : new CultureInfo(item.LangTag);
                        item.NativeName = item.SelectedCulture.NativeName;

                        if(item.SelectedCultureIndex == 0 && !CulturesList.Contains(item.SelectedCulture))
                        {
                            CulturesList.Add(item.SelectedCulture);
                            item.SelectedCultureIndex = CulturesList.IndexOf(item.SelectedCulture);                            
                        }

                        LanguagePreferencesList.Add(item);
                    }
                }

                RaisePropertyChanged(nameof(LanguagePreferencesList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (!appSettings.IsLanguageSet)
            {
                this._appFileSystemService.InitializeFoldersForUser("sources");
                this._appDatabase.Initialize(_appFileSystemService.CurrentUserFolderPath);
            }
        }

        public ICommand RemoveItemCommand => new Command<LangPreferencesItemEntity>(async (item) =>
        {

            if (LanguagePreferencesList.Count == 1)
            {
                await userDialogs.AlertAsync(AppResources.CannotRemove, "Oops");
                return;
            }

            LanguagePreferencesList.Remove(item);

            if (item.LocalId != 0)
                ItemsLangForDelete.Add(item);

        });


        public ICommand AddLangItemCommand => new Command(() =>
        {
            LanguagePreferencesList.Add(new LangPreferencesItemEntity());
        });

        public ICommand OnUpdateLangugeClickedCommand => new Command(async () =>
        {
            foreach(var item in ItemsLangForDelete?.DistinctBy(x => x.LocalId))
            {
                await baseDBService.DeleteAsync(item);
            }

            if (!LanguagePreferencesList.Any(x => x.SelectedCulture == null))
            {
                var itemsLangList = LanguagePreferencesList.DistinctBy(x => x.SelectedCulture);
                foreach (var item in itemsLangList)
                {
                    item.NativeName = item.SelectedCulture.NativeName;
                    item.SelectedCultureIndex = CulturesList.IndexOf(item.SelectedCulture);
                    item.LangTag = item.SelectedCulture.TwoLetterISOLanguageName;

                    if (item.LocalId == 0)
                    {
                        await baseDBService.Add(item);
                    }
                    else
                    {
                        await baseDBService.UpdateAsync(item);
                    }
                }

                appSettings.IsLanguageSet = true;

                await navigationService.NavigateAsync(PageNames.HomePage);
            }
            else if (LanguagePreferencesList.Count != 0)
            {
                await userDialogs.AlertAsync(AppResources.EmptyListRow, AppResources.EmptyRow);
            }
            else
            {
                await userDialogs.AlertAsync(AppResources.EmptyListRow, AppResources.EmptyRow);
            }
        });
    }
}



