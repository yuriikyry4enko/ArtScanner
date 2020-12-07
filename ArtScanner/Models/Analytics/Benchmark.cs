using System;
using System.Diagnostics;
using ArtScanner.Services;

namespace ArtScanner.Models.Analytics
{
    public class Benchmark : IDisposable
    {
        private readonly Stopwatch timer = new Stopwatch();
        private readonly string benchmarkName;

        public Benchmark(string benchmarkName)
        {
            this.benchmarkName = benchmarkName;
            timer.Start();
        }

        public void Dispose()
        {
            timer.Stop();

            TimeSpan timeSpan = timer.Elapsed;
            LogService.Log(null, $"{benchmarkName},  Time: {timeSpan.Hours}h {timeSpan.Minutes}m {timeSpan.Seconds}s {timeSpan.Milliseconds}ms");
        }
    }
}