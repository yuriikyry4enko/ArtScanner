using System;
namespace ArtScanner.Utils.Constants
{
    static class ApiConstants
    {
        //Twitter
        internal static string consumerKey = "nRcidVJCvhwhUZcxnMsLq9GMo";
        internal static string consumerSecret = "L7bD6XwSH87vftpMfD9nhA53RALXMOYxQm97N6r59d6lz6ms4b";

        public const string HOST_CORE = "https://via-net.de";

        public const string GetJPGById = HOST_CORE + "/art-scanner-backend-0-3/item/jpg/{0}";

        public const string GetAudioStreamById = HOST_CORE + "/art-scanner-backend-0-3/item/mp3/{0}/{1}";

        public const string GetTextById = HOST_CORE + "/art-scanner-backend-0-3/item/txt/{0}/{1}";

        public const string GetGeneralItemInfo = HOST_CORE + "/art-scanner-backend-0-3/item/general/{0}";

        public const string GetIdByQRCode = HOST_CORE + "/art-scanner-backend-0-3/qr-code/{0}";

        public const string GetGeneralById = HOST_CORE + "/art-scanner-backend-0-3/item/general/{0}";
    }
}
