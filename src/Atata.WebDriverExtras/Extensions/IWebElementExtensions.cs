using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using OpenQA.Selenium;

namespace Atata
{
    // TODO: Review IWebElementExtensions class. Remove unused methods.
    public static class IWebElementExtensions
    {
        public static WebElementExtendedSearchContext Try(this IWebElement element) =>
            new WebElementExtendedSearchContext(element);

        public static WebElementExtendedSearchContext Try(this IWebElement element, TimeSpan timeout) =>
            new WebElementExtendedSearchContext(element, timeout);

        public static WebElementExtendedSearchContext Try(this IWebElement element, TimeSpan timeout, TimeSpan retryInterval) =>
            new WebElementExtendedSearchContext(element, timeout, retryInterval);

        public static bool HasClass(this IWebElement element, string className) =>
            element.GetAttribute("class").Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Contains(className);

        public static string GetValue(this IWebElement element) =>
            element.GetAttribute("value");

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
                StringBuilder builder = new StringBuilder();

                builder.AppendFormat("- Tag: {0}", element.TagName);

                Point elementLocation = element.Location;
                builder.AppendLine().AppendFormat("- Location: {{X={0}, Y={1}}}", elementLocation.X, elementLocation.Y);

                Size elementSize = element.Size;
                builder.AppendLine().AppendFormat("- Size: {{Width={0}, Height={1}}}", elementSize.Width, elementSize.Height);

                string elementId = element.GetElementId();

                if (!string.IsNullOrEmpty(elementId))
                    builder.AppendLine().AppendFormat("- Element ID: {0}", elementId);

                string elementText = element.Text?.Trim();

                if (!string.IsNullOrEmpty(elementText))
                {
                    string elementTextSplitter = elementText.Contains(Environment.NewLine) ? Environment.NewLine : " ";
                    builder.AppendLine().AppendFormat("- Text:{0}{1}", elementTextSplitter, elementText);
                }

                return builder.ToString();
            }
            catch (WebDriverException)
            {
                return element.ToString();
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
