using System;
using OpenQA.Selenium;

namespace Atata
{
    // TODO: v4. Remove ITimeoutsExtensions.
    public static class ITimeoutsExtensions
    {
        [Obsolete("Instead use: RetrySettings.Timeout = {value};")] // Obsolete since v3.0.0.
        public static ITimeouts SetRetryTimeout(this ITimeouts timeouts, TimeSpan timeToWait)
        {
            RetrySettings.Timeout = timeToWait;
            return timeouts;
        }

        [Obsolete("Instead use: RetrySettings.Timeout = {value}; RetrySettings.Interval = {value};")] // Obsolete since v3.0.0.
        public static ITimeouts SetRetryTimeout(this ITimeouts timeouts, TimeSpan timeToWait, TimeSpan retryInterval)
        {
            RetrySettings.Timeout = timeToWait;
            RetrySettings.Interval = retryInterval;
            return timeouts;
        }
    }
}
