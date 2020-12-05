using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Atata.WebDriverExtras.Tests
{
    public sealed class StopwatchAsserter : IDisposable
    {
        private readonly TimeSpan expectedTime;

        private readonly TimeSpan upperToleranceTime;

        private readonly Stopwatch watch;

        private bool doAssertOnDispose = true;

        public StopwatchAsserter(TimeSpan expectedTime, TimeSpan upperToleranceTime)
        {
            this.expectedTime = expectedTime;
            this.upperToleranceTime = upperToleranceTime;

            watch = Stopwatch.StartNew();
        }

        public static StopwatchAsserter WithinSeconds(double seconds, double upperToleranceSeconds = 1)
        {
            return new StopwatchAsserter(TimeSpan.FromSeconds(seconds), TimeSpan.FromSeconds(upperToleranceSeconds));
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
                Assert.That(watch.Elapsed, Is.InRange(expectedTime, expectedTime + upperToleranceTime));
        }
    }
}
