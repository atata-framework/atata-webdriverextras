namespace Atata;

/// <summary>
/// An exception that is thrown when the expected missing element is actually found.
/// </summary>
[Serializable]
public class ElementNotMissingException : Exception
{
    public ElementNotMissingException()
    {
    }

    public ElementNotMissingException(string? message)
        : base(message)
    {
    }

    public ElementNotMissingException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    protected ElementNotMissingException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public static ElementNotMissingException Create(string? elementName = null, By? by = null, ISearchContext? searchContext = null) =>
        Create(
            new SearchFailureData
            {
                ElementName = elementName,
                By = by,
                SearchContext = searchContext
            });

    public static ElementNotMissingException Create(SearchFailureData searchFailureData)
    {
        string message = (searchFailureData ?? new()).ToStringForElementNotMissing();

        return new(message);
    }
}
