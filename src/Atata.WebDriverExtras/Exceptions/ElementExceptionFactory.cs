namespace Atata;

public static class ElementExceptionFactory
{
    public static ElementNotFoundException CreateForNotFound(string elementName = null, By by = null, ISearchContext searchContext = null) =>
        CreateForNotFound(
            new SearchFailureData
            {
                ElementName = elementName,
                By = by,
                SearchContext = searchContext
            });

    public static ElementNotFoundException CreateForNotFound(SearchFailureData searchFailureData)
    {
        string message = (searchFailureData ?? new SearchFailureData()).ToStringForElementNotFound();

        return new ElementNotFoundException(message);
    }

    public static ElementNotMissingException CreateForNotMissing(string elementName = null, By by = null, ISearchContext searchContext = null) =>
        CreateForNotMissing(
            new SearchFailureData
            {
                ElementName = elementName,
                By = by,
                SearchContext = searchContext
            });

    public static ElementNotMissingException CreateForNotMissing(SearchFailureData searchFailureData)
    {
        string message = (searchFailureData ?? new SearchFailureData()).ToStringForElementNotMissing();

        return new ElementNotMissingException(message);
    }
}
