using System;
namespace ArtScanner.Utils.Constants
{
    static class ApiConstants
    {
        internal static string consumerKey = "nRcidVJCvhwhUZcxnMsLq9GMo";
        internal static string consumerSecret = "L7bD6XwSH87vftpMfD9nhA53RALXMOYxQm97N6r59d6lz6ms4b";

        public const string HOST_CORE = "https://via-net.de";

        public const string GetJPGById = HOST_CORE + "/art-scanner-backend/jpg/{0}";

        public const string GetAudioStreamById = HOST_CORE + "/art-scanner-backend/mp3/{0}";

        public const string GetTextById = HOST_CORE + "/art-scanner-backend/txt/{0}";

    }
}
