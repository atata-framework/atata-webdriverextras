#nullable enable

namespace Atata;

/// <summary>
/// Provides a set of extension methods for <see cref="ISearchContext"/>.
/// </summary>
public static class ISearchContextExtensions
{
    /// <summary>
    /// Gets the first element matching the specified <see cref="By"/> object.
    /// </summary>
    /// <typeparam name="T">The type of the search context.</typeparam>
    /// <param name="searchContext">The search context.</param>
    /// <param name="by">The <see cref="By"/> instance.</param>
    /// <returns>The found element or <see langword="null"/> (if executes safely).</returns>
    public static IWebElement Get<T>(this T searchContext, By by)
        where T : ISearchContext
    {
        var contextToSearchIn = ResolveContext(searchContext);
        return contextToSearchIn.FindElement(by);
    }

    /// <summary>
    /// Gets all the elements matching the specified <see cref="By"/> object.
    /// </summary>
    /// <typeparam name="T">The type of the search context.</typeparam>
    /// <param name="searchContext">The search context.</param>
    /// <param name="by">The <see cref="By"/> instance.</param>
    /// <returns>The collection of found elements.</returns>
    public static ReadOnlyCollection<IWebElement> GetAll<T>(this T searchContext, By by)
        where T : ISearchContext
    {
        var contextToSearchIn = ResolveContext(searchContext);
        return contextToSearchIn.FindElements(by);
    }

    public static bool Exists<T>(this T searchContext, By by)
        where T : ISearchContext
    {
        var contextToSearchIn = ResolveContext(searchContext);
        return contextToSearchIn.Exists(by);
    }

    public static bool Missing<T>(this T searchContext, By by)
        where T : ISearchContext
    {
        var contextToSearchIn = ResolveContext(searchContext);
        return contextToSearchIn.Missing(by);
    }

    private static IExtendedSearchContext ResolveContext<T>(this T searchContext)
        where T : ISearchContext
    {
        searchContext.CheckNotNull(nameof(searchContext));

        if (searchContext.GetType().IsSubclassOfRawGeneric(typeof(ExtendedSearchContext<>)))
            return (IExtendedSearchContext)searchContext;
        else
            return new ExtendedSearchContext<T>(searchContext);
    }
}
