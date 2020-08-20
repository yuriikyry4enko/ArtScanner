using System;
using Xamarin.Essentials;

namespace ArtScanner.Services
{
    public class Settings : ISettings
    {
        private const string IsUserFolderInitializedKey = "IsUserFolderInitializedKey";

        public bool IsUserFolderInitialized
        {
            get => Preferences.Get(IsUserFolderInitializedKey, false);
            set => Preferences.Set(IsUserFolderInitializedKey, value);
        }
    }
}
