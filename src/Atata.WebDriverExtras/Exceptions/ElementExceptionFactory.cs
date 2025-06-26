namespace Atata;

// TODO: v4. Remove ElementExceptionFactory. Move methods to ElementNotFoundException and ElementNotMissingException.
public static class ElementExceptionFactory
{
    [Obsolete("Instead use ElementNotFoundException.Create(...)")] // Obsolete since v4.0.0.
    public static ElementNotFoundException CreateForNotFound(string? elementName = null, By? by = null, ISearchContext? searchContext = null) =>
        ElementNotFoundException.Create(elementName, by, searchContext);

    [Obsolete("Instead use ElementNotFoundException.Create(...)")] // Obsolete since v4.0.0.
    public static ElementNotFoundException CreateForNotFound(SearchFailureData searchFailureData) =>
        ElementNotFoundException.Create(searchFailureData);

    public static ElementNotMissingException CreateForNotMissing(string? elementName = null, By? by = null, ISearchContext? searchContext = null) =>
        CreateForNotMissing(
            new SearchFailureData
            {
                ElementName = elementName,
                By = by,
                SearchContext = searchContext
            });

    public static ElementNotMissingException CreateForNotMissing(SearchFailureData searchFailureData)
    {
        string message = (searchFailureData ?? new()).ToStringForElementNotMissing();

        return new ElementNotMissingException(message);
    }
}
