namespace Atata;

// TODO: Review IWebElementExtensions class. Remove unused methods.
public static class IWebElementExtensions
{
    public static WebElementExtendedSearchContext Try(this IWebElement element) =>
        new(element);

    public static WebElementExtendedSearchContext Try(this IWebElement element, TimeSpan timeout) =>
        new(element, timeout);

    public static WebElementExtendedSearchContext Try(this IWebElement element, TimeSpan timeout, TimeSpan retryInterval) =>
        new(element, timeout, retryInterval);

    public static bool HasClass(this IWebElement element, string className) =>
        element.GetAttribute("class")
            ?.Trim()
            .Split([' '], StringSplitOptions.RemoveEmptyEntries)
            .Contains(className)
            ?? false;

    public static string? GetValue(this IWebElement element) =>
        element.GetAttribute("value");

    public static IWebElement FillInWith(this IWebElement element, string text)
    {
        element.Clear();

        if (text?.Length > 0)
            element.SendKeys(text);

        return element;
    }

    public static string ToDetailedString(this IWebElement element)
    {
        try
        {
            StringBuilder builder = new();

            builder.AppendFormat("- Tag: {0}", element.TagName);

            Point elementLocation = element.Location;
            builder.AppendLine().AppendFormat("- Location: {{X={0}, Y={1}}}", elementLocation.X, elementLocation.Y);

            Size elementSize = element.Size;
            builder.AppendLine().AppendFormat("- Size: {{Width={0}, Height={1}}}", elementSize.Width, elementSize.Height);

            string elementId = element.GetElementId();

            if (elementId?.Length > 0)
                builder.AppendLine().AppendFormat("- Element ID: {0}", elementId);

            string? elementText = element.Text?.Trim();

            if (elementText?.Length > 0)
            {
                string elementTextSplitter = elementText.Contains(Environment.NewLine) ? Environment.NewLine : " ";
                builder.AppendLine().AppendFormat("- Text:{0}{1}", elementTextSplitter, elementText);
            }

            return builder.ToString();
        }
        catch
        {
            return element.ToString() ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets the element identifier.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>The value of element's <c>Id</c> property or <see langword="null"/> if property is missing.</returns>
    public static string GetElementId(this IWebElement element)
    {
        Guard.ThrowIfNull(element);

        PropertyInfo property = element.GetType().GetProperty(
            "Id",
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        return (string)property.GetValue(element, []);
    }
}
