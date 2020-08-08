using System;
using ArtScanner.Services;
using SQLite;

[assembly:Xamarin.Forms.Dependency(typeof(ArtScanner.iOS.Services.SQLite))]
namespace ArtScanner.iOS.Services
{
    class SQLite : ISQLite
    {
        public SQLiteAsyncConnection GetAsyncConnection(string databasePath)
        {
            return new SQLiteAsyncConnection(databasePath);
        }
    }
}