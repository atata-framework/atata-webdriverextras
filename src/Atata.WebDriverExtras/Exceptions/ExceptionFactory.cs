namespace Atata;

/// <summary>
/// Provides a set of methods for an exception creation.
/// </summary>
// TODO: v5. Remove ExceptionFactory.
public static class ExceptionFactory
{
    [Obsolete("Use TimeoutExceptionFactory.Create(...) instead.")] // Obsolete since v4.0.0.
    public static TimeoutException CreateForTimeout(TimeSpan spentTime, Exception? innerException = null) =>
        TimeoutExceptionFactory.Create(spentTime, innerException);

    public static ArgumentException CreateForUnsupportedEnumValue<T>(
        T value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct
    {
        string message = $"Unsupported {typeof(T).FullName} enum value: {value}.";
        return new(message, paramName);
    }
}
