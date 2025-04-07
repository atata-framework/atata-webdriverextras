namespace Atata;

public static class IEnumerableExtensions
{
    // TODO: v4. Make ToReadOnly method obsolete.
    ////[Obsolete("Instead use constructor of ReadOnlyCollection<T> or use another collection type (array, for example).")] // Obsolete since v3.2.0.
    public static ReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> source) =>
        new([.. source]);
}
