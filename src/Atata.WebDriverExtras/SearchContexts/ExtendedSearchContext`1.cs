using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Atata
{
    /// <summary>
    /// Represents an extended context that wraps <see cref="ISearchContext"/>.
    /// </summary>
    /// <typeparam name="T">The type of search context.</typeparam>
    public class ExtendedSearchContext<T> : IExtendedSearchContext
        where T : ISearchContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedSearchContext{T}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public ExtendedSearchContext(T context)
            : this(context, RetrySettings.Timeout)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedSearchContext{T}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="timeout">The timeout.</param>
        public ExtendedSearchContext(T context, TimeSpan timeout)
            : this(context, timeout, RetrySettings.Interval)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedSearchContext{T}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="retryInterval">The retry interval.</param>
        public ExtendedSearchContext(T context, TimeSpan timeout, TimeSpan retryInterval)
        {
            Context = context;
            Timeout = timeout;
            RetryInterval = retryInterval;
        }

        /// <summary>
        /// Gets the actual search context.
        /// </summary>
        public T Context { get; private set; }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// Gets or sets the retry interval.
        /// </summary>
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

        /// <inheritdoc/>
        public IWebElement FindElement(By by)
        {
            return Find(by);
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return FindAll(by);
        }

        /// <inheritdoc/>
        public IWebElement FindElementById(string id)
        {
            return Find(By.Id(id));
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<IWebElement> FindElementsById(string id)
        {
            return FindAll(By.Id(id));
        }

        /// <inheritdoc/>
        public IWebElement FindElementByName(string name)
        {
            return Find(By.Name(name));
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<IWebElement> FindElementsByName(string name)
        {
            return FindAll(By.Name(name));
        }

        /// <inheritdoc/>
        public IWebElement FindElementByTagName(string tagName)
        {
            return Find(By.TagName(tagName));
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<IWebElement> FindElementsByTagName(string tagName)
        {
            return FindAll(By.TagName(tagName));
        }

        /// <inheritdoc/>
        public IWebElement FindElementByClassName(string className)
        {
            return Find(By.ClassName(className));
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<IWebElement> FindElementsByClassName(string className)
        {
            return FindAll(By.ClassName(className));
        }

        /// <inheritdoc/>
        public IWebElement FindElementByLinkText(string linkText)
        {
            return Find(By.LinkText(linkText));
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<IWebElement> FindElementsByLinkText(string linkText)
        {
            return FindAll(By.LinkText(linkText));
        }

        /// <inheritdoc/>
        public IWebElement FindElementByPartialLinkText(string partialLinkText)
        {
            return Find(By.PartialLinkText(partialLinkText));
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<IWebElement> FindElementsByPartialLinkText(string partialLinkText)
        {
            return FindAll(By.PartialLinkText(partialLinkText));
        }

        /// <inheritdoc/>
        public IWebElement FindElementByCssSelector(string cssSelector)
        {
            return Find(By.CssSelector(cssSelector));
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<IWebElement> FindElementsByCssSelector(string cssSelector)
        {
            return FindAll(By.CssSelector(cssSelector));
        }

        /// <inheritdoc/>
        public IWebElement FindElementByXPath(string xpath)
        {
            return Find(By.XPath(xpath));
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<IWebElement> FindElementsByXPath(string xpath)
        {
            return FindAll(By.XPath(xpath));
        }

        private IWebElement Find(By by)
        {
            SearchOptions options = by.GetSearchOptionsOrDefault();

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
            options = options ?? by.GetSearchOptionsOrDefault();

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

            var wait = CreateWait(options);
            return wait.Until(condition);
        }

        public bool Exists(By by)
        {
            return Find(by) != null;
        }

        public bool Missing(By by)
        {
            SearchOptions options = by.GetSearchOptionsOrDefault();

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

            Dictionary<By, SearchOptions> searchOptions = byContextPairs.Keys.ToDictionary(x => x, x => x.GetSearchOptionsOrDefault());

            List<By> leftBys = byContextPairs.Keys.ToList();

            bool FindNoElement(T context)
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

            Stopwatch searchWatch = Stopwatch.StartNew();

            bool isMissing = Until(FindNoElement, maxTimeout, minRetryInterval);

            searchWatch.Stop();

            if (searchOptions.Values.Any(x => !x.IsSafely) && !isMissing)
            {
                By firstLeftBy = leftBys.FirstOrDefault();

                throw ExceptionFactory.CreateForNotMissingElement(
                    new SearchFailureData
                    {
                        By = firstLeftBy,
                        SearchTime = searchWatch.Elapsed,
                        SearchOptions = firstLeftBy != null ? searchOptions[firstLeftBy] : null,
                        SearchContext = firstLeftBy != null ? byContextPairs[firstLeftBy] : null
                    });
            }
            else
            {
                return isMissing;
            }
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
