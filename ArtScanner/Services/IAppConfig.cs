﻿using System;
namespace ArtScanner.Services
{
    interface IAppConfig
    {
        string ServerUrl { get; }

        string RootFolderName { get; }

        string RootPath { get; }

        string ImagesFolderName { get; }

        string RootFolderPath { get; }

        string DatabaseName { get; }

        bool NewDBEachAppVersion { get; }
    }
}