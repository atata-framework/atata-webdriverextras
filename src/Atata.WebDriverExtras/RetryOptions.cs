using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the options for operation that can be retried.
    /// </summary>
    public class RetryOptions
    {
        private static readonly Type StaleElementReferenceExceptionType = typeof(StaleElementReferenceException);

        private TimeSpan? timeout;

        private TimeSpan? interval;

        /// <summary>
        /// Gets or sets the timeout.
        /// The default value is taken from <see cref="RetrySettings.Timeout"/>.
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
        /// Gets or sets the interval.
        /// The default value is taken from <see cref="RetrySettings.Interval"/>.
        /// </summary>
        public TimeSpan Interval
        {
            get => interval ?? RetrySettings.Interval;
            set => interval = value;
        }

        /// <summary>
        /// Gets a value indicating whether <c>Interval</c> is set.
        /// </summary>
        public bool IsIntervalSet => interval.HasValue;

        /// <summary>
        /// Gets the list of ignored exception types.
        /// </summary>
        public List<Type> IgnoredExceptionTypes { get; private set; } = new List<Type>();

        /// <summary>
        /// Sets the timeout.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The same <see cref="RetryOptions"/> instance.</returns>
        public RetryOptions WithTimeout(TimeSpan timeout)
        {
            Timeout = timeout;
            return this;
        }

        /// <summary>
        /// Sets the interval.
        /// </summary>
        /// <param name="interval">The interval.</param>
        /// <returns>The same <see cref="RetryOptions"/> instance.</returns>
        public RetryOptions WithInterval(TimeSpan interval)
        {
            Interval = interval;
            return this;
        }

        /// <summary>
        /// Adds the type of the exception to the list of ignored exception types.
        /// </summary>
        /// <param name="exceptionType">Type of the exception.</param>
        /// <returns>The same <see cref="RetryOptions"/> instance.</returns>
        public RetryOptions IgnoringExceptionType(Type exceptionType)
        {
            IgnoredExceptionTypes.Add(exceptionType);
            return this;
        }

        /// <summary>
        /// Adds <see cref="StaleElementReferenceException"/> type to the list of ignored exception types.
        /// </summary>
        /// <returns>The same <see cref="RetryOptions"/> instance.</returns>
        public RetryOptions IgnoringStaleElementReferenceException()
        {
            IgnoredExceptionTypes.Add(StaleElementReferenceExceptionType);
            return this;
        }
    }
}
