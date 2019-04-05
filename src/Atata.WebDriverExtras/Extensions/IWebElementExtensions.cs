using System;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium;

namespace Atata
{
    // TODO: Review IWebElementExtensions class. Remove unused methods.
    public static class IWebElementExtensions
    {
        public static WebElementExtendedSearchContext Try(this IWebElement element)
        {
            return new WebElementExtendedSearchContext(element);
        }

        public static WebElementExtendedSearchContext Try(this IWebElement element, TimeSpan timeout)
        {
            return new WebElementExtendedSearchContext(element, timeout);
        }

        public static WebElementExtendedSearchContext Try(this IWebElement element, TimeSpan timeout, TimeSpan retryInterval)
        {
            return new WebElementExtendedSearchContext(element, timeout, retryInterval);
        }

        public static bool HasClass(this IWebElement element, string className)
        {
            return element.GetAttribute("class").Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Contains(className);
        }

        public static string GetValue(this IWebElement element)
        {
            return element.GetAttribute("value");
        }

        public static IWebElement FillInWith(this IWebElement element, string text)
        {
            element.Clear();
            if (!string.IsNullOrEmpty(text))
                element.SendKeys(text);
            return element;
        }

        public static string ToDetailedString(this IWebElement element)
        {
            element.CheckNotNull(nameof(element));

            try
            {
                return $@"Tag: {element.TagName}
Location: {element.Location}
Size: {element.Size}
Text: {element.Text.Trim()}";
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the element identifier.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The value of element's <c>Id</c> property or <see langword="null"/> if property is missing.</returns>
        public static string GetElementId(this IWebElement element)
        {
            element.CheckNotNull(nameof(element));

            PropertyInfo property = element.GetType().GetProperty(
                "Id",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            return property?.GetValue(element, new object[0]) as string;
        }
    }
}
