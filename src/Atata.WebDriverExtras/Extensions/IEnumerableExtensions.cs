namespace Atata;

public static class IEnumerableExtensions
{
    public static ReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> source) =>
        new([.. source]);
}
