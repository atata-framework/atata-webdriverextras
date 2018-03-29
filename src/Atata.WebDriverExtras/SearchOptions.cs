using System;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the options for the search of element(s).
    /// </summary>
    public class SearchOptions : ICloneable
    {
        private TimeSpan? timeout;

        private TimeSpan? retryInterval;

        private Visibility? visibility;

        private bool? isSafely;

        /// <summary>
        /// Gets or sets the timeout.
        /// The deafult value is taken from <see cref="RetrySettings.Timeout"/>.
        /// </summary>
        public TimeSpan Timeout
        {
            get => timeout ?? RetrySettings.Timeout;
            set => timeout = value;
        }

        /// <summary>
        /// Gets a value indicating whether <c>Timeout</c> is set.
        /// </summary>
        public bool IsTimeoutSet => timeout.HasValue;

        /// <summary>
        /// Gets or sets the retry interval.
        /// The deafult value is taken from <see cref="RetrySettings.Interval"/>.
        /// </summary>
        public TimeSpan RetryInterval
        {
            get => retryInterval ?? RetrySettings.Interval;
            set => retryInterval = value;
        }

        /// <summary>
        /// Gets a value indicating whether <c>RetryInterval</c> is set.
        /// </summary>
        public bool IsRetryIntervalSet => retryInterval.HasValue;

        /// <summary>
        /// Gets or sets the visibility of the search element.
        /// The default value is <c>Visible</c>.
        /// </summary>
        public Visibility Visibility
        {
            get => visibility ?? Visibility.Visible;
            set => visibility = value;
        }

        /// <summary>
        /// Gets a value indicating whether <c>Visibility</c> is set.
        /// </summary>
        public bool IsVisibilitySet => visibility.HasValue;

        /// <summary>
        /// Gets or sets a value indicating whether the search element is safely searching.
        /// If it is <c>true</c> then <c>null</c> is returned after the search,
        /// otherwise an exception of <see cref="NoSuchElementException"/> or <see cref="NotMissingElementException"/> is thrown.
        /// The default value is <c>false</c>.
        /// </summary>
        public bool IsSafely
        {
            get => isSafely ?? false;
            set => isSafely = value;
        }

        /// <summary>
        /// Gets a value indicating whether <c>IsSafely</c> is set.
        /// </summary>
        public bool IsSafelySet => isSafely.HasValue;

        public static SearchOptions Safely(bool isSafely = true)
        {
            return new SearchOptions { IsSafely = isSafely };
        }

        public static SearchOptions Unsafely()
        {
            return new SearchOptions { IsSafely = false };
        }

        public static SearchOptions SafelyAtOnce(bool isSafely = true)
        {
            return new SearchOptions { IsSafely = isSafely, Timeout = TimeSpan.Zero };
        }

        public static SearchOptions UnsafelyAtOnce()
        {
            return new SearchOptions { IsSafely = false, Timeout = TimeSpan.Zero };
        }

        public static SearchOptions OfVisibility(Visibility visibility)
        {
            return new SearchOptions { Visibility = visibility };
        }

        public static SearchOptions Visible()
        {
            return new SearchOptions { Visibility = Visibility.Visible };
        }

        public static SearchOptions Hidden()
        {
            return new SearchOptions { Visibility = Visibility.Hidden };
        }

        public static SearchOptions OfAnyVisibility()
        {
            return new SearchOptions { Visibility = Visibility.Any };
        }

        public static SearchOptions Within(TimeSpan timeout, TimeSpan? retryInterval = null)
        {
            SearchOptions options = new SearchOptions { Timeout = timeout };

            if (retryInterval.HasValue)
                options.RetryInterval = retryInterval.Value;

            return options;
        }

        public static SearchOptions Within(double timeoutSeconds, double? retryIntervalSeconds = null)
        {
            SearchOptions options = new SearchOptions { Timeout = TimeSpan.FromSeconds(timeoutSeconds) };

            if (retryIntervalSeconds.HasValue)
                options.RetryInterval = TimeSpan.FromSeconds(retryIntervalSeconds.Value);

            return options;
        }

        public static SearchOptions SafelyWithin(TimeSpan timeout, TimeSpan? retryInterval = null)
        {
            SearchOptions options = new SearchOptions { IsSafely = true, Timeout = timeout };

            if (retryInterval.HasValue)
                options.RetryInterval = retryInterval.Value;

            return options;
        }

        public static SearchOptions SafelyWithin(double timeoutSeconds, double? retryIntervalSeconds = null)
        {
            SearchOptions options = new SearchOptions { IsSafely = true, Timeout = TimeSpan.FromSeconds(timeoutSeconds) };

            if (retryIntervalSeconds.HasValue)
                options.RetryInterval = TimeSpan.FromSeconds(retryIntervalSeconds.Value);

            return options;
        }

        public static SearchOptions AtOnce()
        {
            return new SearchOptions { Timeout = TimeSpan.Zero };
        }

        public RetryOptions ToRetryOptions()
        {
            RetryOptions options = new RetryOptions();

            if (IsTimeoutSet)
                options.Timeout = Timeout;

            if (IsRetryIntervalSet)
                options.Interval = RetryInterval;

            return options;
        }

        object ICloneable.Clone() => Clone();

        public SearchOptions Clone()
        {
            return (SearchOptions)MemberwiseClone();
        }
    }
}
