namespace Atata;

/// <summary>
/// Provides a set of extension methods for <see cref="TimeSpan"/>.
/// </summary>
public static class TimeSpanExtensions
{
    /// <summary>
    /// Converts the value of the <see cref="TimeSpan"/> object to its equivalent short interval string representation.
    /// For example, 100 milliseconds value converts to "0.1s".
    /// </summary>
    /// <param name="value">The <see cref="TimeSpan"/> value.</param>
    /// <returns>The short interval string representation of the <see cref="TimeSpan"/> value.</returns>
    public static string ToShortIntervalString(this TimeSpan value) =>
        ToIntervalString(value, "FFF", false);

    /// <summary>
    /// Converts the value of the <see cref="TimeSpan"/> object to its equivalent long interval string representation.
    /// For example, 100 milliseconds value converts to "0.100s".
    /// </summary>
    /// <param name="value">The <see cref="TimeSpan"/> value.</param>
    /// <returns>The long interval string representation of the <see cref="TimeSpan"/> value.</returns>
    public static string ToLongIntervalString(this TimeSpan value) =>
        ToIntervalString(value, "fff", true);

    private static string ToIntervalString(TimeSpan value, string millisecondsFormat, bool secondsRequired)
    {
        StringBuilder builder = new StringBuilder();

        double seconds;

        if (value.TotalMinutes >= 1d)
        {
            double minutes = Math.Floor(value.TotalMinutes);

            builder.AppendFormat("{0:F0}m", minutes);

            seconds = value.TotalSeconds - (minutes * 60d);
        }
        else
        {
            seconds = value.TotalSeconds;
        }

        if (secondsRequired || seconds > 0 || builder.Length == 0)
        {
            if (builder.Length > 0)
                builder.AppendSpace();

            builder.Append(Math.Floor(seconds).ToString("F0"));

            string millisecondsString = value.ToString(millisecondsFormat);

            if (millisecondsString.Length > 0)
                builder.Append('.').Append(millisecondsString);

            builder.Append('s');
        }

        return builder.ToString();
    }
}
