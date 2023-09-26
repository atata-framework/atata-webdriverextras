using System;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Provides a set of methods for an exception creation.
    /// </summary>
    public static class ExceptionFactory
    {
        [Obsolete("Use ElementExceptionFactory.CreateForNotFound(...) instead.")] // Obsolete since v2.3.0.
        public static NoSuchElementException CreateForNoSuchElement(string elementName = null, By by = null, ISearchContext searchContext = null) =>
            CreateForNoSuchElement(
                new SearchFailureData
                {
                    ElementName = elementName,
                    By = by,
                    SearchContext = searchContext
                });

        [Obsolete("Use ElementExceptionFactory.CreateForNotFound(...) instead.")] // Obsolete since v2.3.0.
        public static NoSuchElementException CreateForNoSuchElement(SearchFailureData searchFailureData)
        {
            string message = (searchFailureData ?? new SearchFailureData()).ToStringForNoSuchElement();

            return new NoSuchElementException(message);
        }

        [Obsolete("Use ElementExceptionFactory.CreateForNotMissing(...) instead.")] // Obsolete since v2.3.0.
        public static NotMissingElementException CreateForNotMissingElement(string elementName = null, By by = null, ISearchContext searchContext = null) =>
            CreateForNotMissingElement(
                new SearchFailureData
                {
                    ElementName = elementName,
                    By = by,
                    SearchContext = searchContext
                });

        [Obsolete("Use ElementExceptionFactory.CreateForNotMissing(...) instead.")] // Obsolete since v2.3.0.
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
