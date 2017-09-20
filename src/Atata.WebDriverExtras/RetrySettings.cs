using System;

namespace Atata
{
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

        [ThreadStatic]
        private static TimeSpan? threadStaticTimeout;

        private static TimeSpan? staticTimeout;

        [ThreadStatic]
        private static TimeSpan? threadStaticInterval;

        private static TimeSpan? staticInterval;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Timeout"/> and <see cref="Interval"/> properties use thread-static approach (value unique for each thread).
        /// </summary>
        public static bool IsThreadStatic { get; set; } = true;

        /// <summary>
        /// Gets the retry timeout. The default value is 5 seconds.
        /// </summary>
        public static TimeSpan Timeout
        {
            get => (IsThreadStatic ? threadStaticTimeout : staticTimeout) ?? DefaultTimeout;
            internal set
            {
                if (IsThreadStatic)
                    threadStaticTimeout = value;
                else
                    staticTimeout = value;
            }
        }

        /// <summary>
        /// Gets the retry interval. The default value is 500 milliseconds.
        /// </summary>
        public static TimeSpan Interval
        {
            get => (IsThreadStatic ? threadStaticInterval : staticInterval) ?? DefaultInterval;
            internal set
            {
                if (IsThreadStatic)
                    threadStaticInterval = value;
                else
                    staticInterval = value;
            }
        }
    }
}
