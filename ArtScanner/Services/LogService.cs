using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ArtScanner.Services
{
    public static class LogService
    {
        public static IAnalyticsService AnalyticsService { get; private set; }

        public static void SetAnalyticsService(IAnalyticsService analyticsService)
            => AnalyticsService = analyticsService;

        public static void DebugLine(string info = "", [CallerMemberName] string func = "")
        {
            var record = $"{func} {Environment.NewLine}      {info}";
            Debug.WriteLine(record);
        }

        public static void Log(Exception ex, string info = "", [CallerMemberName] string func = "")
        {
            var record = $"{func} {Environment.NewLine} {info} {Environment.NewLine} : {ex}";
            Debug.WriteLine(record);
            TrackAnalytics(record);
        }

        public static void LogError(string info = "", [CallerMemberName] string func = "")
        {
            var record = $"{func} {Environment.NewLine} {info}";
            Debug.WriteLine(record);
            TrackAnalytics(record);
        }

        static void TrackAnalytics(string record)
            => AnalyticsService?.TrackRecord($"{DateTime.UtcNow.ToString(Utils.Constants.DateTimeFormats.csDateTimeFormat)}___{record}");
    }
}
