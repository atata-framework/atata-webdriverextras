namespace Atata;

/// <summary>
/// Provide a set of static methods to execute an action with retry on <see cref="StaleElementReferenceException"/>.
/// </summary>
public static class StaleSafely
{
    public static TResult Execute<TResult>(Func<TimeSpan, TResult> action, TimeSpan timeout, Action? onExceptionCallback = null)
    {
        action.CheckNotNull(nameof(action));

        TimeSpan workingTimeout = timeout;
        Stopwatch stopwatch = Stopwatch.StartNew();

        while (true)
        {
            try
            {
                return action(workingTimeout);
            }
            catch (StaleElementReferenceException exception)
            {
                onExceptionCallback?.Invoke();

                TimeSpan spentTime = stopwatch.Elapsed;

                if (spentTime > timeout)
                    throw ExceptionFactory.CreateForTimeout(spentTime, exception);
                else
                    workingTimeout = timeout - spentTime;
            }
        }
    }

    public static TResult Execute<TResult>(Func<SearchOptions, TResult> action, SearchOptions options, Action? onExceptionCallback = null)
    {
        action.CheckNotNull(nameof(action));

        options ??= new();

        SearchOptions workingOptions = options.Clone();
        Stopwatch stopwatch = Stopwatch.StartNew();

        while (true)
        {
            try
            {
                return action(workingOptions);
            }
            catch (StaleElementReferenceException exception)
            {
                onExceptionCallback?.Invoke();

                TimeSpan spentTime = stopwatch.Elapsed;

                if (spentTime > options.Timeout)
                    throw ExceptionFactory.CreateForTimeout(spentTime, exception);
                else
                    workingOptions.Timeout = options.Timeout - spentTime;
            }
        }
    }

    public static void Execute(Action<TimeSpan> action, TimeSpan timeout, Action? onExceptionCallback = null)
    {
        action.CheckNotNull(nameof(action));

        TimeSpan workingTimeout = timeout;
        Stopwatch stopwatch = Stopwatch.StartNew();

        while (true)
        {
            try
            {
                action(workingTimeout);
                return;
            }
            catch (StaleElementReferenceException exception)
            {
                onExceptionCallback?.Invoke();

                TimeSpan spentTime = stopwatch.Elapsed;

                if (spentTime > timeout)
                    throw ExceptionFactory.CreateForTimeout(spentTime, exception);
                else
                    workingTimeout = timeout - spentTime;
            }
        }
    }

    public static void Execute(Action<SearchOptions> action, SearchOptions options, Action? onExceptionCallback = null)
    {
        action.CheckNotNull(nameof(action));

        options ??= new();

        SearchOptions workingOptions = options.Clone();
        Stopwatch stopwatch = Stopwatch.StartNew();

        while (true)
        {
            try
            {
                action(workingOptions);
                return;
            }
            catch (StaleElementReferenceException exception)
            {
                onExceptionCallback?.Invoke();

                TimeSpan spentTime = stopwatch.Elapsed;

                if (spentTime > options.Timeout)
                    throw ExceptionFactory.CreateForTimeout(spentTime, exception);
                else
                    workingOptions.Timeout = options.Timeout - spentTime;
            }
        }
    }
}
