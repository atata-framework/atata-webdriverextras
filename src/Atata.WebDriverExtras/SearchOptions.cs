#nullable enable

namespace Atata;

/// <summary>
/// Represents the options for the search of element(s).
/// </summary>
public class SearchOptions : ICloneable
{
    private TimeSpan? _timeout;

    private TimeSpan? _retryInterval;

    private Visibility? _visibility;

    private bool? _isSafely;

    /// <summary>
    /// Gets or sets the default visibility of the search element.
    /// The default value is <see cref="Visibility.Any"/>.
    /// </summary>
    public static Visibility DefaultVisibility { get; set; }

    /// <summary>
    /// Gets or sets the timeout.
    /// The default value is taken from <see cref="RetrySettings.Timeout"/>.
    /// </summary>
    public TimeSpan Timeout
    {
        get => _timeout ?? RetrySettings.Timeout;
        set => _timeout = value;
    }

    /// <summary>
    /// Gets a value indicating whether <see cref="Timeout"/> is set.
    /// </summary>
    public bool IsTimeoutSet => _timeout.HasValue;

    /// <summary>
    /// Gets or sets the retry interval.
    /// The default value is taken from <see cref="RetrySettings.Interval"/>.
    /// </summary>
    public TimeSpan RetryInterval
    {
        get => _retryInterval ?? RetrySettings.Interval;
        set => _retryInterval = value;
    }

    /// <summary>
    /// Gets a value indicating whether <see cref="RetryInterval"/> is set.
    /// </summary>
    public bool IsRetryIntervalSet => _retryInterval.HasValue;

    /// <summary>
    /// Gets or sets the visibility of the search element.
    /// The default value is taken from <see cref="DefaultVisibility"/> property,
    /// which is <see cref="Visibility.Any"/> by default.
    /// </summary>
    public Visibility Visibility
    {
        get => _visibility ?? DefaultVisibility;
        set => _visibility = value;
    }

    /// <summary>
    /// Gets a value indicating whether <see cref="Visibility"/> is set.
    /// </summary>
    public bool IsVisibilitySet => _visibility.HasValue;

    /// <summary>
    /// Gets or sets a value indicating whether the search element is safely searching.
    /// If it is <c>true</c> then <c>null</c> is returned after the search,
    /// otherwise an exception of <see cref="ElementNotFoundException"/> or <see cref="ElementNotMissingException"/> is thrown.
    /// The default value is <c>false</c>.
    /// </summary>
    public bool IsSafely
    {
        get => _isSafely ?? false;
        set => _isSafely = value;
    }

    /// <summary>
    /// Gets a value indicating whether <see cref="IsSafely"/> is set.
    /// </summary>
    public bool IsSafelySet => _isSafely.HasValue;

    public static SearchOptions Safely(bool isSafely = true) =>
        new() { IsSafely = isSafely };

    public static SearchOptions Unsafely() =>
        new() { IsSafely = false };

    public static SearchOptions SafelyAtOnce(bool isSafely = true) =>
        new() { IsSafely = isSafely, Timeout = TimeSpan.Zero };

    public static SearchOptions UnsafelyAtOnce() =>
        new() { IsSafely = false, Timeout = TimeSpan.Zero };

    public static SearchOptions OfVisibility(Visibility visibility) =>
        new() { Visibility = visibility };

    public static SearchOptions Visible() =>
        new() { Visibility = Visibility.Visible };

    public static SearchOptions Hidden() =>
        new() { Visibility = Visibility.Hidden };

    public static SearchOptions OfAnyVisibility() =>
        new() { Visibility = Visibility.Any };

    public static SearchOptions Within(TimeSpan timeout, TimeSpan? retryInterval = null)
    {
        SearchOptions options = new() { Timeout = timeout };

        if (retryInterval.HasValue)
            options.RetryInterval = retryInterval.Value;

        return options;
    }

    public static SearchOptions Within(double timeoutSeconds, double? retryIntervalSeconds = null)
    {
        SearchOptions options = new() { Timeout = TimeSpan.FromSeconds(timeoutSeconds) };

        if (retryIntervalSeconds.HasValue)
            options.RetryInterval = TimeSpan.FromSeconds(retryIntervalSeconds.Value);

        return options;
    }

    public static SearchOptions SafelyWithin(TimeSpan timeout, TimeSpan? retryInterval = null)
    {
        SearchOptions options = new() { IsSafely = true, Timeout = timeout };

        if (retryInterval.HasValue)
            options.RetryInterval = retryInterval.Value;

        return options;
    }

    public static SearchOptions SafelyWithin(double timeoutSeconds, double? retryIntervalSeconds = null)
    {
        SearchOptions options = new() { IsSafely = true, Timeout = TimeSpan.FromSeconds(timeoutSeconds) };

        if (retryIntervalSeconds.HasValue)
            options.RetryInterval = TimeSpan.FromSeconds(retryIntervalSeconds.Value);

        return options;
    }

    public static SearchOptions AtOnce() =>
        new() { Timeout = TimeSpan.Zero };

    public RetryOptions ToRetryOptions()
    {
        RetryOptions options = new();

        if (IsTimeoutSet)
            options.Timeout = Timeout;

        if (IsRetryIntervalSet)
            options.Interval = RetryInterval;

        return options;
    }

    /// <inheritdoc cref="Clone"/>
    object ICloneable.Clone() => Clone();

    /// <summary>
    /// Creates a new object that is a copy of the current instance.
    /// </summary>
    /// <returns>A new object that is a copy of this instance.</returns>
    public SearchOptions Clone() =>
        (SearchOptions)MemberwiseClone();

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A <see cref="string"/> that represents this instance.</returns>
    public override string ToString() =>
        $"{{{nameof(Visibility)}={Visibility}, {nameof(Timeout)}={Timeout.ToShortIntervalString()}, {nameof(RetryInterval)}={RetryInterval.ToShortIntervalString()}, {nameof(IsSafely)}={IsSafely}}}";
}
