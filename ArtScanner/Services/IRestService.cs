﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace ArtScanner.Services
{
    interface IRestService
    {
        Task<byte[]> GetMusicStreamById(string id);
        Task<byte[]> GetImageById(string id);
        Task<string> GetTextById(string id);

    }
}