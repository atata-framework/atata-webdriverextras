using System;

namespace Atata
{
    /// <summary>
    /// Provides the default settings for operations that can be retried.
    /// </summary>
    public static class RetrySettings
    {
        /// <summary>
        /// The default timeout is 5 seconds.
        /// </summary>
        public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);

        /// <summary>
        /// The default interval is 500 milliseconds.
        /// </summary>
        public static readonly TimeSpan DefaultInterval = TimeSpan.FromSeconds(0.5);

#if NET46 || NETSTANDARD2_0
        private static readonly System.Threading.AsyncLocal<TimeoutIntervalPair> AsyncLocalSettings = new System.Threading.AsyncLocal<TimeoutIntervalPair>();
#endif

        private static TimeoutIntervalPair staticSettings;

        [ThreadStatic]
        private static TimeoutIntervalPair threadStaticSettings;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Timeout"/> and <see cref="Interval"/> properties use thread-static approach (value unique for each thread).
        /// </summary>
        [Obsolete("Use ThreadBoundary instead.")] // Obsolete since v1.3.0.
        public static bool IsThreadStatic
        {
            get => ThreadBoundary == RetrySettingsThreadBoundary.ThreadStatic;
            set => ThreadBoundary = value ? RetrySettingsThreadBoundary.ThreadStatic : RetrySettingsThreadBoundary.Static;
        }

        /// <summary>
        /// Gets or sets the thread boundary of <see cref="RetrySettings"/>.
        /// </summary>
        public static RetrySettingsThreadBoundary ThreadBoundary { get; set; } = RetrySettingsThreadBoundary.ThreadStatic;

        /// <summary>
        /// Gets the retry timeout.
        /// The default value is 5 seconds.
        /// </summary>
        public static TimeSpan Timeout
        {
            get => ResolveCurrentSettings().TimeoutValue ?? DefaultTimeout;
            internal set => ResolveCurrentSettings().TimeoutValue = value;
        }

        /// <summary>
        /// Gets the retry interval.
        /// The default value is 500 milliseconds.
        /// </summary>
        public static TimeSpan Interval
        {
            get => ResolveCurrentSettings().IntervalValue ?? DefaultInterval;
            internal set => ResolveCurrentSettings().IntervalValue = value;
        }

        private static TimeoutIntervalPair ResolveCurrentSettings()
        {
            switch (ThreadBoundary)
            {
                case RetrySettingsThreadBoundary.ThreadStatic:
                    return threadStaticSettings ?? (threadStaticSettings = new TimeoutIntervalPair());
                case RetrySettingsThreadBoundary.Static:
                    return staticSettings ?? (staticSettings = new TimeoutIntervalPair());
#if NET46 || NETSTANDARD2_0
                case RetrySettingsThreadBoundary.AsyncLocal:
                    return AsyncLocalSettings.Value ?? (AsyncLocalSettings.Value = new TimeoutIntervalPair());
#endif
                default:
                    throw new InvalidOperationException($"Unknown {nameof(ThreadBoundary)}={ThreadBoundary} value.");
            }
        }

        private class TimeoutIntervalPair
        {
            public TimeSpan? TimeoutValue { get; set; }

            public TimeSpan? IntervalValue { get; set; }
        }
    }
}
