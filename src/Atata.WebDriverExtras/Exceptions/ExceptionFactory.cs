using System.Runtime.CompilerServices;

namespace Atata;

/// <summary>
/// Provides a set of methods for an exception creation.
/// </summary>
// TODO: v4. Remove ExceptionFactory.
public static class ExceptionFactory
{
    public static TimeoutException CreateForTimeout(TimeSpan spentTime, Exception? innerException = null)
    {
        string message = $"Timed out after {spentTime.TotalSeconds} seconds.";
        return new TimeoutException(message, innerException);
    }

    public static ArgumentException CreateForUnsupportedEnumValue<T>(
        T value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct
    {
        string message = $"Unsupported {typeof(T).FullName} enum value: {value}.";
        return new ArgumentException(message, paramName);
    }
}
