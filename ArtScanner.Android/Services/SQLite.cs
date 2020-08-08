using System;
using ArtScanner.Services;
using SQLite;

[assembly: Xamarin.Forms.Dependency(typeof(ArtScanner.Droid.Services.SQLite))]
namespace ArtScanner.Droid.Services
{
    class SQLite : ISQLite
    {
        public SQLiteAsyncConnection GetAsyncConnection(string databasePath)
        {
            return new SQLiteAsyncConnection(databasePath);
        }
    }
}   