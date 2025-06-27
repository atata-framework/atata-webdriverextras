namespace Atata;

// TODO: v5. Remove IEnumerableExtensions.
public static class IEnumerableExtensions
{
    [Obsolete("Instead use constructor of ReadOnlyCollection<T> or use another collection type (array, for example).")] // Obsolete since v4.0.0.
    public static ReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> source) =>
        new([.. source]);
}
