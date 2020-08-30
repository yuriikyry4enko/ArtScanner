using System;
namespace ArtScanner.Services
{
    public interface IAppSettings 
    {
        bool IsLanguageSet { get; set; }
        string LanguagePreferences { get; set; }
    }
}
