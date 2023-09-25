using System;
using System.Runtime.Serialization;

namespace Atata
{
    /// <summary>
    /// The exception that is thrown during a waiting for a particular element to be missing, but it was present.
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
}
