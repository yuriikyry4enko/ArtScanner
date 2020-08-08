using System;
namespace ArtScanner.Services
{
    interface IAppInfo
    {
        string Version { get; }
        string Build { get; }
    }
}
