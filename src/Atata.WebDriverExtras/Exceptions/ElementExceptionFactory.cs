namespace Atata;

// TODO: v5. Remove ElementExceptionFactory.
public static class ElementExceptionFactory
{
    [Obsolete("Instead use ElementNotFoundException.Create(...)")] // Obsolete since v4.0.0.
    public static ElementNotFoundException CreateForNotFound(string? elementName = null, By? by = null, ISearchContext? searchContext = null) =>
        ElementNotFoundException.Create(elementName, by, searchContext);

    [Obsolete("Instead use ElementNotFoundException.Create(...)")] // Obsolete since v4.0.0.
    public static ElementNotFoundException CreateForNotFound(SearchFailureData searchFailureData) =>
        ElementNotFoundException.Create(searchFailureData);

    [Obsolete("Instead use ElementNotMissingException.Create(...)")] // Obsolete since v4.0.0.
    public static ElementNotMissingException CreateForNotMissing(string? elementName = null, By? by = null, ISearchContext? searchContext = null) =>
        ElementNotMissingException.Create(elementName, by, searchContext);

    [Obsolete("Instead use ElementNotMissingException.Create(...)")] // Obsolete since v4.0.0.
    public static ElementNotMissingException CreateForNotMissing(SearchFailureData searchFailureData) =>
        ElementNotMissingException.Create(searchFailureData);
}
