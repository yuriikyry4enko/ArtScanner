using System;
using System.IO;

namespace ArtScanner.Utils.Constants
{
    static class ApiConstants
    {
        //Twitter
        internal static string consumerKey = "nRcidVJCvhwhUZcxnMsLq9GMo";
        internal static string consumerSecret = "L7bD6XwSH87vftpMfD9nhA53RALXMOYxQm97N6r59d6lz6ms4b";

        public const string HOST_CORE = "https://via-net.de/art-scanner-backend-0-5";

        public const string GetJPGById = HOST_CORE + "/item/jpg/{0}";

        public const string GetAudioStreamById = HOST_CORE + "/item/mp3/{0}/{1}";

        public const string GetTextById = HOST_CORE + "/item/txt/{0}/{1}";

        public const string GetGeneralItemInfo = HOST_CORE + "/item/general/{0}";

        public const string GetIdByQRCode = HOST_CORE + "/qr-code/{0}";

        public const string GetGeneralById = HOST_CORE + "/item/general/{0}";

        public const string GetItemIdBYShortCode = HOST_CORE + "/item/short-id/{0}/{1}";
    }

    static class AppConstants
    {
        public static string csHomePageUpdate = "HomePageUpdate";
                 
        //public static string csLocalAnalyticsFilePath =
        //         Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "logs.txt");


        public static string csSpecialFolder =
                 Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal));

        //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "logs.txt");


        public static string csEmailSupport = "yuriykiri4enko@gmail.com";
        public static string csEmailSupportSubject = "Logs from SmartBooklet";
        public static string csEmailSupportBody = $"It is auto-generated email with attached log file from SmartBooklet app.{Environment.NewLine}It will help us to improve application and resolve issues.";

    }

    public static class DateTimeFormats
    {
        public const string csTimeFormat = @"hh\:mm";
        public static string csDateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
        public const string csEmptyTimeFormat = "00:00";
    }
}
