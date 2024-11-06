namespace Atata;

/// <summary>
/// Represents the retriable operation to wait for condition safely (without throwing exception on timeout).
/// </summary>
/// <typeparam name="T">The type of object used to detect the condition.</typeparam>
public class SafeWait<T> : IWait<T>
{
    private readonly T _input;

    private readonly IClock _clock;

    private readonly List<Type> _ignoredExceptions = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="SafeWait{T}"/> class.
    /// </summary>
    /// <param name="input">The input value to pass to the evaluated conditions.</param>
    public SafeWait(T input)
        : this(input, new SystemClock())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SafeWait{T}"/> class.
    /// </summary>
    /// <param name="input">The input value to pass to the evaluated conditions.</param>
    /// <param name="clock">The clock to use when measuring the timeout.</param>
    public SafeWait(T input, IClock clock)
    {
        _input = input.CheckNotNull(nameof(input));
        _clock = clock.CheckNotNull(nameof(clock));
    }

    /// <summary>
    /// Gets or sets how long to wait for the evaluated condition to be true.
    /// The default timeout is taken from <see cref="RetrySettings.Timeout"/>.
    /// </summary>
    public TimeSpan Timeout { get; set; } = RetrySettings.Timeout;

    /// <summary>
    /// Gets or sets how often the condition should be evaluated.
    /// The default interval is taken from <see cref="RetrySettings.Interval"/>.
    /// </summary>
    public TimeSpan PollingInterval { get; set; } = RetrySettings.Interval;

    public string Message { get; set; }

    /// <summary>
    /// Configures this instance to ignore specific types of exceptions while waiting for a condition.
    /// Any exceptions not whitelisted will be allowed to propagate, terminating the wait.
    /// </summary>
    /// <param name="exceptionTypes">The types of exceptions to ignore.</param>
    public void IgnoreExceptionTypes(params Type[] exceptionTypes)
    {
        exceptionTypes.CheckNotNull(nameof(exceptionTypes));

        // Commented out due to performance. It is absolutely not critical to have inappropriate type passed.
        ////foreach (Type exceptionType in exceptionTypes)
        ////{
        ////    if (!typeof(Exception).IsAssignableFrom(exceptionType))
        ////    {
        ////        throw new ArgumentException("All types to be ignored must derive from System.Exception", "exceptionTypes");
        ////    }
        ////}

        _ignoredExceptions.AddRange(exceptionTypes);
    }

    /// <summary>
    /// Repeatedly applies this instance's input value to the given function until one of the following
    /// occurs:
    /// <para>
    /// <list type="bullet">
    /// <item>the function returns neither null nor false</item>
    /// <item>the function throws an exception that is not in the list of ignored exception types</item>
    /// <item>the timeout expires</item>
    /// </list>
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The delegate's expected return type.</typeparam>
    /// <param name="condition">A delegate taking an object of type T as its parameter, and returning a TResult.</param>
    /// <returns>The delegate's return value.</returns>
    public TResult Until<TResult>(Func<T, TResult> condition)
    {
        condition.CheckNotNull(nameof(condition));

        DateTime operationStart = _clock.Now;
        DateTime operationTimeoutEnd = operationStart.Add(Timeout);

        while (true)
        {
            DateTime iterationStart = _clock.Now;

            try
            {
                var result = condition(_input);

                if (DoesConditionResultSatisfy(result))
                    return result;
            }
            catch (Exception exception)
            {
                if (!IsIgnoredException(exception))
                    throw;
            }

            DateTime iterationEnd = _clock.Now;
            TimeSpan timeUntilTimeout = operationTimeoutEnd - iterationEnd;

            if (timeUntilTimeout <= TimeSpan.Zero)
            {
                if (typeof(TResult) == typeof(ReadOnlyCollection<IWebElement>))
                    return (TResult)(object)new IWebElement[0].ToReadOnly();
                else if (typeof(TResult) == typeof(IWebElement[]))
                    return (TResult)(object)new IWebElement[0];
                else
                    return default;
            }
            else
            {
                TimeSpan timeToSleep = PollingInterval - (iterationEnd - iterationStart);

                if (timeUntilTimeout < timeToSleep)
                    timeToSleep = timeUntilTimeout;

                if (timeToSleep > TimeSpan.Zero)
                    Thread.Sleep(timeToSleep);
            }
        }
    }

    protected virtual bool DoesConditionResultSatisfy<TResult>(TResult result)
    {
        if (typeof(TResult) == typeof(bool))
        {
            var boolResult = result as bool?;

            if (boolResult.HasValue && boolResult.Value)
                return true;
        }
        else if (result is not null && (result is not IEnumerable || ((IEnumerable)result).Cast<object>().Any()))
        {
            return true;
        }

        return false;
    }

    private bool IsIgnoredException(Exception exception) =>
        _ignoredExceptions.Exists(type => type.IsAssignableFrom(exception.GetType()));
}
