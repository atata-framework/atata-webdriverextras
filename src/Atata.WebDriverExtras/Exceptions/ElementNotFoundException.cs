namespace Atata;

/// <summary>
/// An exception that is thrown when the searched element is not found.
/// </summary>
public class ElementNotFoundException : Exception
{
    public ElementNotFoundException()
    {
    }

    public ElementNotFoundException(string? message)
        : base(message)
    {
    }

    public ElementNotFoundException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public static ElementNotFoundException Create(string? elementName = null, By? by = null, ISearchContext? searchContext = null) =>
        Create(
            new SearchFailureData
            {
                ElementName = elementName,
                By = by,
                SearchContext = searchContext
            });

    public static ElementNotFoundException Create(SearchFailureData searchFailureData)
    {
        string message = (searchFailureData ?? new SearchFailureData()).ToStringForElementNotFound();

        return new(message);
    }
}
