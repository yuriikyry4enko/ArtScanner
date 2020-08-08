using System;
using SQLite;

namespace ArtScanner.Services
{
    public interface ISQLite
    {
        SQLiteAsyncConnection GetAsyncConnection(string databasePath);
    }
}
