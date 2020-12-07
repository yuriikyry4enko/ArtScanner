using System;
namespace ArtScanner.Utils.Constants
{
    static class PageNames
    {
        public static string HomePage = nameof(Views.HomePage);
        public static string StartPage = nameof(Views.StartPage);
        public static string ScannerPage = nameof(Views.ScannerPage);
        public static string ItemsGalleryPage = nameof(Views.ItemsGalleryPage);
        public static string ItemsGalleryDetailsPage = nameof(Views.ItemGalleryDetailsPage);
        public static string ProviderLoginPage = nameof(Views.ProviderLoginPage);
        public static string ChooseLanguagePage = nameof(Views.ChooseLanguagePage);

        public static string ApologizeLanguagePopupPage = nameof(Popups.ApologizeLanguagePopupPage);
        public static string LoadingPopupPage = nameof(Popups.LoadingPopupPage);
        public static string BurgerMenuPopupPage = nameof(Popups.BurgerMenuPopupPage);
        public static string BookletItemDetailsPage = nameof(Views.BookletItemDetailsPage);
        public static string SettingsPage = nameof(Views.SettingsPage);
    }
}
