namespace Atata;

/// <summary>
/// Provides factory methods for creating <see cref="TimeoutException"/> instances.
/// </summary>
public static class TimeoutExceptionFactory
{
    /// <summary>
    /// Creates a <see cref="TimeoutException"/> with a message in a following format:
    /// <c>"Timed out after {time}."</c>.
    /// </summary>
    /// <param name="spentTime">The time interval that was spent before timing out.</param>
    /// <param name="innerException">The exception that caused the current exception, or <see langword="null"/> if no inner exception is specified.</param>
    /// <returns>
    /// A <see cref="TimeoutException"/> initialized with a formatted message and optional inner exception.
    /// </returns>
    public static TimeoutException Create(TimeSpan spentTime, Exception? innerException = null)
    {
        string message = $"Timed out after {spentTime.ToShortIntervalString()}.";
        return new(message, innerException);
    }
}
