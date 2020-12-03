using System;
namespace ArtScanner.Models.Analytics
{
    public abstract class AnalyticsServiceProvider
    {
        public abstract void WriteRecord(string record);
    }
}
