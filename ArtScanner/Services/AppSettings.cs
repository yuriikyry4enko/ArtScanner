using System;
using Xamarin.Essentials;

namespace ArtScanner.Services
{
    public class AppSettings : IAppSettings
    {
        private const string IsLanguageSetKey = "IsLanguageSetKey";

        public bool IsLanguageSet
        {
            get => Preferences.Get(IsLanguageSetKey, false);
            set => Preferences.Set(IsLanguageSetKey, value);
        }

        private const string LanguagePreferencesKey = "LanguagePreferencesKey";

        public string LanguagePreferences
        {
            get => Preferences.Get(LanguagePreferencesKey, string.Empty);
            set => Preferences.Set(LanguagePreferencesKey, value);
        }
    }
}
