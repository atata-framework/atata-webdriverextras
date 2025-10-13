namespace Atata;

/// <summary>
/// Provide a set of static methods to execute an action with retry on
/// <see cref="StaleElementReferenceException"/> or <see cref="UnknownErrorException"/>.
/// </summary>
public static class StaleSafely
{
    public static TResult Execute<TResult>(Func<TimeSpan, TResult> action, TimeSpan timeout, Action? onExceptionCallback = null)
    {
        Guard.ThrowIfNull(action);

        TimeSpan workingTimeout = timeout;
        Stopwatch stopwatch = Stopwatch.StartNew();

        while (true)
        {
            try
            {
                return action(workingTimeout);
            }
            catch (Exception exception) when (exception is StaleElementReferenceException or UnknownErrorException)
            {
                onExceptionCallback?.Invoke();

                TimeSpan spentTime = stopwatch.Elapsed;

                if (spentTime > timeout)
                    throw TimeoutExceptionFactory.Create(spentTime, exception);
                else
                    workingTimeout = timeout - spentTime;
            }
        }
    }

    public static TResult Execute<TResult>(Func<SearchOptions, TResult> action, SearchOptions options, Action? onExceptionCallback = null)
    {
        Guard.ThrowIfNull(action);

        options ??= new();

        SearchOptions workingOptions = options.Clone();
        Stopwatch stopwatch = Stopwatch.StartNew();

        while (true)
        {
            try
            {
                return action(workingOptions);
            }
            catch (Exception exception) when (exception is StaleElementReferenceException or UnknownErrorException)
            {
                onExceptionCallback?.Invoke();

                TimeSpan spentTime = stopwatch.Elapsed;

                if (spentTime > options.Timeout)
                    throw TimeoutExceptionFactory.Create(spentTime, exception);
                else
                    workingOptions.Timeout = options.Timeout - spentTime;
            }
        }
    }

    public static void Execute(Action<TimeSpan> action, TimeSpan timeout, Action? onExceptionCallback = null)
    {
        Guard.ThrowIfNull(action);

        TimeSpan workingTimeout = timeout;
        Stopwatch stopwatch = Stopwatch.StartNew();

        while (true)
        {
            try
            {
                action(workingTimeout);
                return;
            }
            catch (Exception exception) when (exception is StaleElementReferenceException or UnknownErrorException)
            {
                onExceptionCallback?.Invoke();

                TimeSpan spentTime = stopwatch.Elapsed;

                if (spentTime > timeout)
                    throw TimeoutExceptionFactory.Create(spentTime, exception);
                else
                    workingTimeout = timeout - spentTime;
            }
        }
    }

    public static void Execute(Action<SearchOptions> action, SearchOptions options, Action? onExceptionCallback = null)
    {
        Guard.ThrowIfNull(action);

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
            catch (Exception exception) when (exception is StaleElementReferenceException or UnknownErrorException)
            {
                onExceptionCallback?.Invoke();

                TimeSpan spentTime = stopwatch.Elapsed;

                if (spentTime > options.Timeout)
                    throw TimeoutExceptionFactory.Create(spentTime, exception);
                else
                    workingOptions.Timeout = options.Timeout - spentTime;
            }
        }
    }
}
