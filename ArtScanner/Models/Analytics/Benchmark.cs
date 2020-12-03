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
            Console.WriteLine("{0},  Time: {1}h {2}m {3}s {4}ms", benchmarkName, timeSpan.Hours, timeSpan.Minutes,
            timeSpan.Seconds, timeSpan.Milliseconds);

            //LogService.Log(null, $"{benchmarkName} {timer.Elapsed}");
            //Console.WriteLine($"{benchmarkName} {timer.Elapsed}");
        }
    }
}