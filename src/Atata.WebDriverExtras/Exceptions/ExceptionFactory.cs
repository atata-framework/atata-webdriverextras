using System;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Provides a set of methods for an exception creation.
    /// </summary>
    public static class ExceptionFactory
    {
        public static NoSuchElementException CreateForNoSuchElement(string elementName = null, By by = null, ISearchContext searchContext = null) =>
            CreateForNoSuchElement(
                new SearchFailureData
                {
                    ElementName = elementName,
                    By = by,
                    SearchContext = searchContext
                });

        /// <summary>
        /// Creates an instance of <see cref="NoSuchElementException"/> with message generated using <paramref name="searchFailureData"/>.
        /// </summary>
        /// <param name="searchFailureData">The search failure data.</param>
        /// <returns>An instance of <see cref="NoSuchElementException"/>.</returns>
        public static NoSuchElementException CreateForNoSuchElement(SearchFailureData searchFailureData)
        {
            string message = (searchFailureData ?? new SearchFailureData()).ToStringForNoSuchElement();

            return new NoSuchElementException(message);
        }

        public static NotMissingElementException CreateForNotMissingElement(string elementName = null, By by = null, ISearchContext searchContext = null) =>
            CreateForNotMissingElement(
                new SearchFailureData
                {
                    ElementName = elementName,
                    By = by,
                    SearchContext = searchContext
                });

        /// <summary>
        /// Creates an instance of <see cref="NotMissingElementException"/> with message generated using <paramref name="searchFailureData"/>.
        /// </summary>
        /// <param name="searchFailureData">The search failure data.</param>
        /// <returns>An instance of <see cref="NotMissingElementException"/>.</returns>
        public static NotMissingElementException CreateForNotMissingElement(SearchFailureData searchFailureData)
        {
            string message = (searchFailureData ?? new SearchFailureData()).ToStringForNotMissingElement();

            return new NotMissingElementException(message);
        }

        public static TimeoutException CreateForTimeout(TimeSpan spentTime, Exception innerException = null)
        {
            string message = $"Timed out after {spentTime.TotalSeconds} seconds.";
            return new TimeoutException(message, innerException);
        }

        public static ArgumentException CreateForUnsupportedEnumValue<T>(T value, string paramName)
            where T : struct
        {
            string message = $"Unsupported {typeof(T).FullName} enum value: {value}.";
            return new ArgumentException(message, paramName);
        }
    }
}
