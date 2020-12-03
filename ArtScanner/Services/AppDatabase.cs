using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using ArtScanner.Models.Entities;
using SQLite;

namespace ArtScanner.Services
{
    class AppDatabase : IAppDatabase
    {
        private readonly ISQLite sqLite;
        private readonly IAppConfig appConfig;
        private readonly IAppInfo appcInfo;

        public SQLiteAsyncConnection Connection { get; private set; }

        public async Task Initialize(string path)
        {
            try
            {
                if (Connection != null)
                {
                    await Connection.CloseAsync();
                }

                string databasePath;

                if (appConfig.NewDBEachAppVersion)
                {
                    var databaseName = $"{appConfig.DatabaseName}_{appcInfo.Version}.{appcInfo.Build}.db";

                    databasePath = Path.Combine(path, databaseName);

                    // remove all previous dbs
                    foreach (var fileFullPath in Directory.EnumerateFiles(path, "*.db"))
                    {
                        var filename = Path.GetFileName(fileFullPath);

                        if (filename != databaseName)
                        {
                            File.Delete(fileFullPath);
                        }
                    }
                }
                else
                {
                    databasePath = Path.Combine(path, $"{appConfig.DatabaseName}.db");
                }

                Connection = sqLite.GetAsyncConnection(databasePath);

                await Connection.CreateTableAsync<ItemEntity>();
                await Connection.CreateTableAsync<LangPreferencesItemEntity>();
            }
            catch (Exception ex)
            {
                LogService.Log(ex);
            }
        }

        public AppDatabase(
            ISQLite sqLite,
            IAppConfig appConfig,
            IAppInfo appInfo)
        {
            this.appConfig = appConfig;
            this.sqLite = sqLite;
            this.appcInfo = appInfo;
        }
    }
}
