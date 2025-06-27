namespace Atata;

// TODO: v5. Remove ElementExceptionFactory.
public static class ElementExceptionFactory
{
    [Obsolete("Use ElementNotFoundException.Create(...) instead.")] // Obsolete since v4.0.0.
    public static ElementNotFoundException CreateForNotFound(string? elementName = null, By? by = null, ISearchContext? searchContext = null) =>
        ElementNotFoundException.Create(elementName, by, searchContext);

    [Obsolete("Use ElementNotFoundException.Create(...) instead.")] // Obsolete since v4.0.0.
    public static ElementNotFoundException CreateForNotFound(SearchFailureData searchFailureData) =>
        ElementNotFoundException.Create(searchFailureData);

    [Obsolete("Use ElementNotMissingException.Create(...) instead.")] // Obsolete since v4.0.0.
    public static ElementNotMissingException CreateForNotMissing(string? elementName = null, By? by = null, ISearchContext? searchContext = null) =>
        ElementNotMissingException.Create(elementName, by, searchContext);

    [Obsolete("Use ElementNotMissingException.Create(...) instead.")] // Obsolete since v4.0.0.
    public static ElementNotMissingException CreateForNotMissing(SearchFailureData searchFailureData) =>
        ElementNotMissingException.Create(searchFailureData);
}
