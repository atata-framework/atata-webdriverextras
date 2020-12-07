using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Atata.WebDriverExtras.Tests
{
    public sealed class StopwatchAsserter : IDisposable
    {
        private readonly TimeSpan expectedTime;

        private readonly TimeSpan upperToleranceTime;

        private readonly TimeSpan lowerToleranceTime;

        private readonly Stopwatch watch;

        private bool doAssertOnDispose = true;

        public StopwatchAsserter(TimeSpan expectedTime, TimeSpan upperToleranceTime)
            : this(expectedTime, upperToleranceTime, TimeSpan.Zero)
        {
        }

        public StopwatchAsserter(TimeSpan expectedTime, TimeSpan upperToleranceTime, TimeSpan lowerToleranceTime)
        {
            this.expectedTime = expectedTime;
            this.upperToleranceTime = upperToleranceTime;
            this.lowerToleranceTime = lowerToleranceTime;

            watch = Stopwatch.StartNew();
        }

        public static StopwatchAsserter WithinSeconds(double seconds, double upperToleranceSeconds = 1, double lowerToleranceSeconds = 0.001)
        {
            return new StopwatchAsserter(
                TimeSpan.FromSeconds(seconds),
                TimeSpan.FromSeconds(upperToleranceSeconds),
                TimeSpan.FromSeconds(lowerToleranceSeconds));
        }

        public TResult Execute<TResult>(Func<TResult> function)
        {
            try
            {
                return function.Invoke();
            }
            catch (Exception)
            {
                doAssertOnDispose = false;
                throw;
            }
            finally
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            watch.Stop();

            if (doAssertOnDispose)
                Assert.That(watch.Elapsed, Is.InRange(expectedTime - lowerToleranceTime, expectedTime + upperToleranceTime));
        }
    }
}
