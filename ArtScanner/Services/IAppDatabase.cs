using System;
using System.Threading.Tasks;
using SQLite;

namespace ArtScanner.Services
{
    interface IAppDatabase
    {
        SQLiteAsyncConnection Connection { get; }
        Task Initialize(string path);
    }
}
