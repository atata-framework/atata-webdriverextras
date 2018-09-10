using System;
using System.Threading;
using NUnit.Framework;

namespace Atata.WebDriverExtras.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.None)]
    public class SafeWaitTests
    {
        private SafeWait<object> wait;

        [SetUp]
        public void SetUp()
        {
            wait = new SafeWait<object>(new object())
            {
                Timeout = TimeSpan.FromSeconds(.3),
                PollingInterval = TimeSpan.FromSeconds(.05)
            };
        }

        [Test]
        public void SafeWait_Success_Immediate()
        {
            using (StopwatchAsserter.Within(0, .01))
                wait.Until(_ =>
                {
                    return true;
                });
        }

        [Test]
        public void SafeWait_Timeout()
        {
            using (StopwatchAsserter.Within(.3, .01))
                wait.Until(_ =>
                {
                    return false;
                });
        }

        [Test]
        public void SafeWait_PollingInterval()
        {
            using (StopwatchAsserter.Within(.3, .2))
                wait.Until(_ =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(.1));
                    return false;
                });
        }

        [Test]
        public void SafeWait_PollingInterval_GreaterThanTimeout()
        {
            wait.PollingInterval = TimeSpan.FromSeconds(1);

            using (StopwatchAsserter.Within(.3, .01))
                wait.Until(_ =>
                {
                    return false;
                });
        }
    }
}
