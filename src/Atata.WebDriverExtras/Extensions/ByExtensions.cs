#nullable enable

namespace Atata;

public static class ByExtensions
{
    public static By OfKind(this By by, string kind, string? name = null)
    {
        ExtendedBy extendedBy = new(by) { ElementKind = kind };

        return name is not null
            ? extendedBy.Named(name)
            : extendedBy;
    }

    public static By Named(this By by, string name)
    {
        ExtendedBy extendedBy = new(by) { ElementName = name };

        return name is not null && extendedBy.ToString().Contains("{0}")
            ? extendedBy.FormatWith(name)
            : extendedBy;
    }

    public static By Safely(this By by, bool isSafely = true)
    {
        ExtendedBy extendedBy = new(by);
        extendedBy.Options.IsSafely = isSafely;
        return extendedBy;
    }

    public static By Unsafely(this By by)
    {
        ExtendedBy extendedBy = new(by);
        extendedBy.Options.IsSafely = false;
        return extendedBy;
    }

    public static By Visible(this By by) =>
        by.With(Visibility.Visible);

    public static By Hidden(this By by) =>
        by.With(Visibility.Hidden);

    public static By OfAnyVisibility(this By by) =>
        by.With(Visibility.Any);

    public static By Within(this By by, TimeSpan timeout)
    {
        ExtendedBy extendedBy = new(by);
        extendedBy.Options.Timeout = timeout;
        return extendedBy;
    }

    public static By Within(this By by, TimeSpan timeout, TimeSpan retryInterval)
    {
        ExtendedBy extendedBy = new(by);
        extendedBy.Options.Timeout = timeout;
        extendedBy.Options.RetryInterval = retryInterval;
        return extendedBy;
    }

    public static By AtOnce(this By by)
    {
        ExtendedBy extendedBy = new(by);
        extendedBy.Options.Timeout = TimeSpan.Zero;
        return extendedBy;
    }

    public static By SafelyAtOnce(this By by, bool isSafely = true)
    {
        ExtendedBy extendedBy = new(by);
        extendedBy.Options.IsSafely = isSafely;
        extendedBy.Options.Timeout = TimeSpan.Zero;
        return extendedBy;
    }

    public static By With(this By by, Visibility visibility)
    {
        ExtendedBy extendedBy = new(by);
        extendedBy.Options.Visibility = visibility;
        return extendedBy;
    }

    public static By With(this By by, SearchOptions options)
    {
        options ??= new();

        ExtendedBy extendedBy = new(by);

        if (options.IsTimeoutSet)
            extendedBy.Options.Timeout = options.Timeout;

        if (options.IsRetryIntervalSet)
            extendedBy.Options.RetryInterval = options.RetryInterval;

        if (options.IsVisibilitySet)
            extendedBy.Options.Visibility = options.Visibility;

        if (options.IsSafelySet)
            extendedBy.Options.IsSafely = options.IsSafely;

        return extendedBy;
    }

    public static By FormatWith(this By by, params object[] args)
    {
        By formattedBy;

        if (TryResolveByChain(by, out ByChain? byChain))
        {
            formattedBy = new ByChain(byChain.Items.Select(x => x.FormatWith(args)));
        }
        else
        {
            string selector = string.Format(by.GetSelector(), args);
            formattedBy = CreateBy(by.GetMethod(), selector);
        }

        return by is ExtendedBy originalByAsExtended
            ? new ExtendedBy(formattedBy).ApplySettingsFrom(originalByAsExtended)
            : formattedBy;
    }

    private static bool TryResolveByChain(By by, [MaybeNullWhen(false)] out ByChain byChain)
    {
        byChain = (by as ByChain) ?? (by as ExtendedBy)?.By as ByChain;
        return byChain is not null;
    }

    private static string GetMethod(this By by) =>
        by.ToString().Split(':')[0].Replace("By.", string.Empty);

    private static string GetSelector(this By by)
    {
        string text = by.ToString();
        return text[(text.IndexOf(':') + 2)..];
    }

    private static By CreateBy(string method, string selector) =>
        method switch
        {
            "Id" => By.Id(selector),
            "LinkText" => By.LinkText(selector),
            "Name" => By.Name(selector),
            "XPath" => By.XPath(selector),
            "ClassName[Contains]" => By.ClassName(selector),
            "PartialLinkText" => By.PartialLinkText(selector),
            "TagName" => By.TagName(selector),
            "CssSelector" => By.CssSelector(selector),
            _ => throw new ArgumentException($@"Unknown ""{method}"" method of OpenQA.Selenium.By.", nameof(method)),
        };

    public static By Then(this By by, By nextBy)
    {
        ExtendedBy? originalByAsExtended = by as ExtendedBy;

        ByChain newByChain = TryResolveByChain(by, out ByChain? byChain)
            ? new(byChain.Items.Concat([nextBy]))
            : new(originalByAsExtended?.By ?? by, nextBy);

        return originalByAsExtended is not null
            ? new ExtendedBy(newByChain).ApplySettingsFrom(originalByAsExtended)
            : newByChain;
    }

    /// <summary>
    /// Gets the search options associated with <paramref name="by"/> or default search options.
    /// </summary>
    /// <param name="by">The by.</param>
    /// <returns>The search options.</returns>
    public static SearchOptions GetSearchOptionsOrDefault(this By by) =>
        (by as ExtendedBy)?.Options ?? new SearchOptions();

    /// <summary>
    /// Converts to descriptive string.
    /// Example: <c>XPath "//div"</c>.
    /// </summary>
    /// <param name="by">The by.</param>
    /// <returns>The string.</returns>
    public static string ToDescriptiveString(this By by)
    {
        By actualBy = (by as ExtendedBy)?.By ?? by;

        if (actualBy is ByChain byChain)
        {
            return $"chain [{string.Join($", ", byChain.Items.Select(ToDescriptiveString))}]";
        }
        else
        {
            string byAsString = actualBy.ToString();
            int indexOfColon = byAsString.IndexOf(':');

            string method = byAsString[..indexOfColon];
            string selector = byAsString[(byAsString.IndexOf(':') + 2)..];

            return $"{StringifyByMethod(method)} \"{selector}\"";
        }
    }

    private static string StringifyByMethod(string method) =>
        method switch
        {
            "By.Id" => "id",
            "By.LinkText" => "link text",
            "By.Name" => "name",
            "By.XPath" => "XPath",
            "By.ClassName[Contains]" => "class",
            "By.PartialLinkText" => "partial link text",
            "By.TagName" => "tag name",
            "By.CssSelector" => "CSS selector",
            _ => method,
        };
}
