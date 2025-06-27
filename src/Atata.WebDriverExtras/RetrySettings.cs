namespace Atata;

/// <summary>
/// Provides the default settings for operations that can be retried.
/// </summary>
public static class RetrySettings
{
    /// <summary>
    /// The default timeout is 5 seconds.
    /// </summary>
    public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);

    /// <summary>
    /// The default interval is 200 milliseconds.
    /// </summary>
    public static readonly TimeSpan DefaultInterval = TimeSpan.FromMilliseconds(200);

    private static readonly AsyncLocal<TimeoutIntervalPair> s_asyncLocalSettings = new();

    private static TimeoutIntervalPair? s_staticSettings;

    [ThreadStatic]
    private static TimeoutIntervalPair? s_threadStaticSettings;

    /// <summary>
    /// Gets or sets the thread boundary of <see cref="RetrySettings"/>.
    /// The default value is <see cref="RetrySettingsThreadBoundary.AsyncLocal"/>.
    /// </summary>
    public static RetrySettingsThreadBoundary ThreadBoundary { get; set; } = RetrySettingsThreadBoundary.AsyncLocal;

    /// <summary>
    /// Gets or sets the retry timeout.
    /// The default value is 5 seconds.
    /// </summary>
    public static TimeSpan Timeout
    {
        get => ResolveCurrentSettings().TimeoutValue ?? DefaultTimeout;
        set => ResolveCurrentSettings().TimeoutValue = value;
    }

    /// <summary>
    /// Gets or sets the retry interval.
    /// The default value is 200 milliseconds.
    /// </summary>
    public static TimeSpan Interval
    {
        get => ResolveCurrentSettings().IntervalValue ?? DefaultInterval;
        set => ResolveCurrentSettings().IntervalValue = value;
    }

    private static TimeoutIntervalPair ResolveCurrentSettings()
    {
        switch (ThreadBoundary)
        {
            case RetrySettingsThreadBoundary.ThreadStatic:
                return s_threadStaticSettings ??= new();
            case RetrySettingsThreadBoundary.Static:
                return s_staticSettings ??= new();
            case RetrySettingsThreadBoundary.AsyncLocal:
                return s_asyncLocalSettings.Value ??= new();
            default:
                throw new InvalidOperationException($"Unknown {nameof(ThreadBoundary)}={ThreadBoundary} value.");
        }
    }

    private sealed class TimeoutIntervalPair
    {
        public TimeSpan? TimeoutValue { get; set; }

        public TimeSpan? IntervalValue { get; set; }
    }
}
