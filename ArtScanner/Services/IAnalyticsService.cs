using System;
using ArtScanner.Models.Analytics;

namespace ArtScanner.Services
{
    public interface IAnalyticsService
    {
        void TrackRecord(string message);
        void SetProvider(AnalyticsServiceProvider provider);
    }
}
