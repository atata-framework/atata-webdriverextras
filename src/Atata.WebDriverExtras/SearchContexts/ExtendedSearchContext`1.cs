namespace Atata;

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

    private static Func<IWebElement, bool> CreateVisibilityPredicate(Visibility visibility) =>
        visibility switch
        {
            Visibility.Visible => x => x.Displayed,
            Visibility.Hidden => x => !x.Displayed,
            Visibility.Any => x => true,
            _ => throw Guard.CreateArgumentExceptionForUnsupportedValue(visibility)
        };

    IWebElement ISearchContext.FindElement(By by) =>
        FindElement(by)!;

    /// <summary>
    /// Finds the first <see cref="IWebElement"/> using the given method.
    /// </summary>
    /// <param name="by">The locating mechanism to use.</param>
    /// <returns>The first matching <see cref="IWebElement"/> on the current context.</returns>
    /// <exception cref="ElementNotFoundException">If no element matches the criteria.</exception>
    public IWebElement? FindElement(By by) =>
        Find(by);

    /// <summary>
    /// Finds all <see cref="IWebElement">IWebElements</see> within the current context using the given mechanism.
    /// </summary>
    /// <param name="by">The locating mechanism to use.</param>
    /// <returns>
    /// A <see cref="ReadOnlyCollection{T}"/> of all <see cref="IWebElement">WebElements</see>
    /// matching the current criteria, or an empty list if nothing matches.
    /// </returns>
    public ReadOnlyCollection<IWebElement> FindElements(By by) =>
        FindAll(by);

    private IWebElement? Find(By by)
    {
        SearchOptions options = by.GetSearchOptionsOrDefault();

        IWebElement? FindElement(T context) =>
            context.FindElement(by);

        ReadOnlyCollection<IWebElement>? lastFoundElements = null;

        IWebElement? FindElementWithVisibilityFiltering(T context)
        {
            lastFoundElements = context.FindElements(by);

            return lastFoundElements.FirstOrDefault(CreateVisibilityPredicate(options.Visibility));
        }

        RetryOptions retryOptions = options.ToRetryOptions();
        Stopwatch searchWatch = Stopwatch.StartNew();

        IWebElement? element = options.Visibility == Visibility.Any
            ? Until(FindElement, retryOptions.IgnoringExceptionType(typeof(NoSuchElementException)))
            : Until(FindElementWithVisibilityFiltering, retryOptions);

        searchWatch.Stop();

        if (!options.IsSafely && element is null)
        {
            throw ElementNotFoundException.Create(
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

    private ReadOnlyCollection<IWebElement> FindAll(By by, SearchOptions? options = null)
    {
        options ??= by.GetSearchOptionsOrDefault();

        Func<T, ReadOnlyCollection<IWebElement>> findFunction;

        if (options.Visibility == Visibility.Any)
        {
            findFunction = x => x.FindElements(by);
        }
        else
        {
            findFunction = x => new(
                [.. x.FindElements(by).Where(CreateVisibilityPredicate(options.Visibility))]);
        }

        return Until(findFunction, options.ToRetryOptions())!;
    }

    public TResult? Until<TResult>(Func<T, TResult> condition, TimeSpan? timeout = null, TimeSpan? retryInterval = null)
    {
        RetryOptions options = new();

        if (timeout.HasValue)
            options.Timeout = timeout.Value;

        if (retryInterval.HasValue)
            options.Interval = retryInterval.Value;

        return Until(condition, options);
    }

    public TResult? Until<TResult>(Func<T, TResult> condition, RetryOptions options)
    {
        if (condition is null)
            throw new ArgumentNullException(nameof(condition));

        options ??= new();

        if (!options.IsTimeoutSet)
            options.Timeout = Timeout;

        if (!options.IsIntervalSet)
            options.Interval = RetryInterval;

        var wait = CreateWait(options);
        return wait.Until(condition);
    }

    public bool Exists(By by) =>
        Find(by) != null;

    public bool Missing(By by)
    {
        SearchOptions options = by.GetSearchOptionsOrDefault();

        bool FindNoElement(T context) =>
            options.Visibility == Visibility.Any
                ? context.FindElements(by).Count == 0
                : !context.FindElements(by).Any(CreateVisibilityPredicate(options.Visibility));

        Stopwatch searchWatch = Stopwatch.StartNew();

        bool isMissing = Until(FindNoElement, options.ToRetryOptions());

        searchWatch.Stop();

        if (!options.IsSafely && !isMissing)
        {
            throw ElementNotMissingException.Create(
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

    public bool MissingAll(params By[] byArray) =>
        MissingAll(byArray.ToDictionary(x => x, x => (ISearchContext)Context));

    public bool MissingAll(Dictionary<By, ISearchContext> byContextPairs)
    {
        Guard.ThrowIfNullOrEmpty(byContextPairs);

        Dictionary<By, SearchOptions> searchOptions = byContextPairs.Keys.ToDictionary(x => x, x => x.GetSearchOptionsOrDefault());

        List<By> leftBys = [.. byContextPairs.Keys];

        bool FindNoElement(T context)
        {
            By[] currentByArray = [.. leftBys];

            foreach (By by in currentByArray)
            {
                if (IsMissing(byContextPairs[by], by, searchOptions[by]))
                    leftBys.Remove(by);
            }

            if (leftBys.Count == 0)
            {
                leftBys = [.. byContextPairs.Keys
                    .Except(currentByArray)
                    .Where(by => !IsMissing(byContextPairs[by], by, searchOptions[by]))];

                if (leftBys.Count == 0)
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

            throw ElementNotMissingException.Create(
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

    private static bool IsMissing(ISearchContext context, By by, SearchOptions options) =>
        !context.FindElements(by).Any(CreateVisibilityPredicate(options.Visibility));

    private SafeWait<T> CreateWait(RetryOptions options)
    {
        SafeWait<T> wait = new(Context)
        {
            Timeout = options.Timeout,
            PollingInterval = options.Interval
        };

        foreach (Type exceptionType in options.IgnoredExceptionTypes)
            wait.IgnoreExceptionTypes(exceptionType);

        return wait;
    }
}
