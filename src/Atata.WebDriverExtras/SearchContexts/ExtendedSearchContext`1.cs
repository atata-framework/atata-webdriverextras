using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Atata
{
    public class ExtendedSearchContext<T> : IExtendedSearchContext
        where T : ISearchContext
    {
        public ExtendedSearchContext(T context)
            : this(context, RetrySettings.Timeout)
        {
        }

        public ExtendedSearchContext(T context, TimeSpan timeout)
            : this(context, timeout, RetrySettings.Interval)
        {
        }

        public ExtendedSearchContext(T context, TimeSpan timeout, TimeSpan retryInterval)
        {
            Context = context;
            Timeout = timeout;
            RetryInterval = retryInterval;
        }

        public T Context { get; private set; }

        public TimeSpan Timeout { get; set; }

        public TimeSpan RetryInterval { get; set; }

        private static Func<IWebElement, bool> CreateVisibilityPredicate(Visibility visibility)
        {
            switch (visibility)
            {
                case Visibility.Visible:
                    return x => x.Displayed;
                case Visibility.Hidden:
                    return x => !x.Displayed;
                case Visibility.Any:
                    return x => true;
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue<Visibility>(visibility, nameof(visibility));
            }
        }

        public IWebElement FindElement(By by)
        {
            return Find(by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return FindAll(by);
        }

        public IWebElement FindElementById(string id)
        {
            return Find(By.Id(id));
        }

        public ReadOnlyCollection<IWebElement> FindElementsById(string id)
        {
            return FindAll(By.Id(id));
        }

        public IWebElement FindElementByName(string name)
        {
            return Find(By.Name(name));
        }

        public ReadOnlyCollection<IWebElement> FindElementsByName(string name)
        {
            return FindAll(By.Name(name));
        }

        public IWebElement FindElementByTagName(string tagName)
        {
            return Find(By.TagName(tagName));
        }

        public ReadOnlyCollection<IWebElement> FindElementsByTagName(string tagName)
        {
            return FindAll(By.TagName(tagName));
        }

        public IWebElement FindElementByClassName(string className)
        {
            return Find(By.ClassName(className));
        }

        public ReadOnlyCollection<IWebElement> FindElementsByClassName(string className)
        {
            return FindAll(By.ClassName(className));
        }

        public IWebElement FindElementByLinkText(string linkText)
        {
            return Find(By.LinkText(linkText));
        }

        public ReadOnlyCollection<IWebElement> FindElementsByLinkText(string linkText)
        {
            return FindAll(By.LinkText(linkText));
        }

        public IWebElement FindElementByPartialLinkText(string partialLinkText)
        {
            return Find(By.PartialLinkText(partialLinkText));
        }

        public ReadOnlyCollection<IWebElement> FindElementsByPartialLinkText(string partialLinkText)
        {
            return FindAll(By.PartialLinkText(partialLinkText));
        }

        public IWebElement FindElementByCssSelector(string cssSelector)
        {
            return Find(By.CssSelector(cssSelector));
        }

        public ReadOnlyCollection<IWebElement> FindElementsByCssSelector(string cssSelector)
        {
            return FindAll(By.CssSelector(cssSelector));
        }

        public IWebElement FindElementByXPath(string xpath)
        {
            return Find(By.XPath(xpath));
        }

        public ReadOnlyCollection<IWebElement> FindElementsByXPath(string xpath)
        {
            return FindAll(By.XPath(xpath));
        }

        private static SearchOptions ResolveOptions(By by)
        {
            return (by as ExtendedBy)?.Options ?? new SearchOptions();
        }

        private IWebElement Find(By by)
        {
            SearchOptions options = ResolveOptions(by);

            ReadOnlyCollection<IWebElement> lastFoundElements = null;

            IWebElement FindElement(T context)
            {
                lastFoundElements = context.FindElements(by);

                return options.Visibility == Visibility.Any
                    ? lastFoundElements.FirstOrDefault()
                    : lastFoundElements.FirstOrDefault(CreateVisibilityPredicate(options.Visibility));
            }

            Stopwatch searchWatch = Stopwatch.StartNew();

            IWebElement element = Until(FindElement, options.ToRetryOptions());

            searchWatch.Stop();

            if (!options.IsSafely && element == null)
            {
                throw ExceptionFactory.CreateForNoSuchElement(
                    new SearchFailureData
                    {
                        By = by,
                        SearchTime = searchWatch.Elapsed,
                        SearchOptions = options,
                        AlikeElementsWithInverseVisibility = lastFoundElements,
                        SearchContext = Context
                    });
            }
            else
            {
                return element;
            }
        }

        private ReadOnlyCollection<IWebElement> FindAll(By by, SearchOptions options = null)
        {
            options = options ?? ResolveOptions(by);

            Func<T, ReadOnlyCollection<IWebElement>> findFunction;

            if (options.Visibility == Visibility.Any)
            {
                findFunction = x => x.FindElements(by);
            }
            else
            {
                findFunction = x => x.FindElements(by).
                    Where(CreateVisibilityPredicate(options.Visibility)).
                    ToReadOnly();
            }

            return Until(findFunction, options.ToRetryOptions());
        }

        public TResult Until<TResult>(Func<T, TResult> condition, TimeSpan? timeout = null, TimeSpan? retryInterval = null)
        {
            RetryOptions options = new RetryOptions();

            if (timeout.HasValue)
                options.Timeout = timeout.Value;

            if (retryInterval.HasValue)
                options.Interval = retryInterval.Value;

            return Until(condition, options);
        }

        public TResult Until<TResult>(Func<T, TResult> condition, RetryOptions options)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            options = options ?? new RetryOptions();

            if (!options.IsTimeoutSet)
                options.Timeout = Timeout;

            if (!options.IsIntervalSet)
                options.Interval = RetryInterval;

            if (options.Timeout > TimeSpan.Zero)
            {
                var wait = CreateWait(options);
                return wait.Until(condition);
            }
            else
            {
                return condition(Context);
            }
        }

        public bool Exists(By by)
        {
            return Find(by) != null;
        }

        public bool Missing(By by)
        {
            SearchOptions options = ResolveOptions(by);

            bool FindNoElement(T context)
            {
                return options.Visibility == Visibility.Any
                    ? !context.FindElements(by).Any()
                    : !context.FindElements(by).Any(CreateVisibilityPredicate(options.Visibility));
            }

            Stopwatch searchWatch = Stopwatch.StartNew();

            bool isMissing = Until(FindNoElement, options.ToRetryOptions());

            searchWatch.Stop();

            if (!options.IsSafely && !isMissing)
            {
                throw ExceptionFactory.CreateForNotMissingElement(
                    new SearchFailureData
                    {
                        By = by,
                        SearchTime = searchWatch.Elapsed,
                        SearchOptions = options,
                        SearchContext = Context
                    });
            }
            else
            {
                return isMissing;
            }
        }

        public bool MissingAll(params By[] byArray)
        {
            return MissingAll(byArray.ToDictionary(x => x, x => (ISearchContext)Context));
        }

        public bool MissingAll(Dictionary<By, ISearchContext> byContextPairs)
        {
            byContextPairs.CheckNotNullOrEmpty(nameof(byContextPairs));

            Dictionary<By, SearchOptions> searchOptions = byContextPairs.Keys.ToDictionary(x => x, x => ResolveOptions(x));

            List<By> leftBys = byContextPairs.Keys.ToList();

            bool findFunction(T context)
            {
                By[] currentByArray = leftBys.ToArray();

                foreach (By by in currentByArray)
                {
                    if (IsMissing(byContextPairs[by], by, searchOptions[by]))
                        leftBys.Remove(by);
                }

                if (!leftBys.Any())
                {
                    leftBys = byContextPairs.Keys.Except(currentByArray).Where(by => !IsMissing(byContextPairs[by], by, searchOptions[by])).ToList();
                    if (!leftBys.Any())
                        return true;
                }

                return false;
            }

            TimeSpan? maxTimeout = searchOptions.Values.Where(x => x.IsTimeoutSet).Max(x => x.Timeout as TimeSpan?);
            TimeSpan? minRetryInterval = searchOptions.Values.Where(x => x.IsRetryIntervalSet).Min(x => x.RetryInterval as TimeSpan?);

            bool isMissing = Until(findFunction, maxTimeout, minRetryInterval);

            if (searchOptions.Values.Any(x => !x.IsSafely) && !isMissing)
                throw ExceptionFactory.CreateForNotMissingElement(by: leftBys.FirstOrDefault(), searchContext: Context);
            else
                return isMissing;
        }

        private static bool IsMissing(ISearchContext context, By by, SearchOptions options)
        {
            return !context.FindElements(by).Any(CreateVisibilityPredicate(options.Visibility));
        }

        private IWait<T> CreateWait(RetryOptions options)
        {
            IWait<T> wait = new SafeWait<T>(Context)
            {
                Timeout = options.Timeout,
                PollingInterval = options.Interval
            };

            foreach (Type exceptionType in options.IgnoredExceptionTypes)
                wait.IgnoreExceptionTypes(exceptionType);

            return wait;
        }
    }
}
