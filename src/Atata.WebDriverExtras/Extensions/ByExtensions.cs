using System;
using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    public static class ByExtensions
    {
        public static By OfKind(this By by, string kind, string name = null)
        {
            ExtendedBy extendedBy = new ExtendedBy(by) { ElementKind = kind };
            return name != null ? extendedBy.Named(name) : extendedBy;
        }

        public static By Named(this By by, string name)
        {
            ExtendedBy extendedBy = new ExtendedBy(by) { ElementName = name };

            if (name != null && extendedBy.ToString().Contains("{0}"))
            {
                return extendedBy.FormatWith(name);
            }

            return extendedBy;
        }

        public static By Safely(this By by, bool isSafely = true)
        {
            ExtendedBy extendedBy = new ExtendedBy(by);
            extendedBy.Options.IsSafely = isSafely;
            return extendedBy;
        }

        public static By Unsafely(this By by)
        {
            ExtendedBy extendedBy = new ExtendedBy(by);
            extendedBy.Options.IsSafely = false;
            return extendedBy;
        }

        public static By Visible(this By by)
        {
            return by.With(Visibility.Visible);
        }

        public static By Hidden(this By by)
        {
            return by.With(Visibility.Hidden);
        }

        public static By OfAnyVisibility(this By by)
        {
            return by.With(Visibility.Any);
        }

        public static By With(this By by, Visibility visibility)
        {
            ExtendedBy extendedBy = new ExtendedBy(by);
            extendedBy.Options.Visibility = visibility;
            return extendedBy;
        }

        public static By Within(this By by, TimeSpan timeout)
        {
            ExtendedBy extendedBy = new ExtendedBy(by);
            extendedBy.Options.Timeout = timeout;
            return extendedBy;
        }

        public static By Within(this By by, TimeSpan timeout, TimeSpan retryInterval)
        {
            ExtendedBy extendedBy = new ExtendedBy(by);
            extendedBy.Options.Timeout = timeout;
            extendedBy.Options.RetryInterval = retryInterval;
            return extendedBy;
        }

        public static By AtOnce(this By by)
        {
            ExtendedBy extendedBy = new ExtendedBy(by);
            extendedBy.Options.Timeout = TimeSpan.Zero;
            return extendedBy;
        }

        public static By SafelyAtOnce(this By by, bool isSafely = true)
        {
            ExtendedBy extendedBy = new ExtendedBy(by);
            extendedBy.Options.IsSafely = isSafely;
            extendedBy.Options.Timeout = TimeSpan.Zero;
            return extendedBy;
        }

        public static By With(this By by, SearchOptions options)
        {
            options = options ?? new SearchOptions();

            ExtendedBy extendedBy = new ExtendedBy(by);

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

            if (TryResolveByChain(by, out ByChain byChain))
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

        private static bool TryResolveByChain(By by, out ByChain byChain)
        {
            byChain = (by as ByChain) ?? (by as ExtendedBy)?.By as ByChain;
            return byChain != null;
        }

        private static string GetMethod(this By by)
        {
            return by.ToString().Split(':')[0].Replace("By.", string.Empty);
        }

        private static string GetSelector(this By by)
        {
            string text = by.ToString();
            return text.Substring(text.IndexOf(':') + 2);
        }

        private static By CreateBy(string method, string selector)
        {
            switch (method)
            {
                case "Id":
                    return By.Id(selector);
                case "LinkText":
                    return By.LinkText(selector);
                case "Name":
                    return By.Name(selector);
                case "XPath":
                    return By.XPath(selector);
                case "ClassName[Contains]":
                    return By.ClassName(selector);
                case "PartialLinkText":
                    return By.PartialLinkText(selector);
                case "TagName":
                    return By.TagName(selector);
                case "CssSelector":
                    return By.CssSelector(selector);
                default:
                    throw new ArgumentException($"Unknown {method} method of OpenQA.Selenium.By.", nameof(method));
            }
        }

        public static By Then(this By by, By nextBy)
        {
            ExtendedBy originalByAsExtended = by as ExtendedBy;

            By newByChain = TryResolveByChain(by, out ByChain byChain)
                ? new ByChain(byChain.Items.Concat(new[] { nextBy }))
                : new ByChain(originalByAsExtended?.By ?? by, nextBy);

            return originalByAsExtended != null
                ? new ExtendedBy(newByChain).ApplySettingsFrom(originalByAsExtended)
                : newByChain;
        }

        /// <summary>
        /// Gets the search options associated with <paramref name="by"/> or default search options.
        /// </summary>
        /// <param name="by">The by.</param>
        /// <returns>The search options.</returns>
        public static SearchOptions GetSearchOptionsOrDefault(this By by)
        {
            return (by as ExtendedBy)?.Options ?? new SearchOptions();
        }
    }
}
