﻿namespace Atata;

/// <summary>
/// An exception that is thrown when the searched element is not found.
/// </summary>
[Serializable]
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

    protected ElementNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
