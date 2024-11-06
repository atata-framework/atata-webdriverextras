namespace Atata;

/// <summary>
/// The exception that is thrown when the expected missing element is actually found.
/// </summary>
[Serializable]
public class ElementNotMissingException : Exception
{
    public ElementNotMissingException()
    {
    }

    public ElementNotMissingException(string message)
        : base(message)
    {
    }

    public ElementNotMissingException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected ElementNotMissingException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
