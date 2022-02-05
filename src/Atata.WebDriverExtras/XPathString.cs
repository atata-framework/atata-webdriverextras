using System.Linq;
using System.Text;

namespace Atata
{
    /// <summary>
    /// Represents XPath string value.
    /// </summary>
    public class XPathString
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XPathString"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public XPathString(string value)
        {
            Value = ConvertTo(value);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="XPathString"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(XPathString value)
        {
            return value.Value;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="string"/> to <see cref="XPathString"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator XPathString(string value)
        {
            return new XPathString(value);
        }

        /// <summary>
        /// Converts to XPath valid string wrapping the value with <c>'</c> or <c>"</c> characters.
        /// For string containing both <c>'</c> and <c>"</c> characters applies XPath <c>concat</c> function.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns>The valid XPath string.</returns>
        public static string ConvertTo(string value)
        {
            return value != null && value.Contains("'")
                ? value.Contains("\"")
                    ? NormalizeHybridQuotedString(value)
                    : $"\"{value}\""
                : $"'{value}'";
        }

        private static string NormalizeHybridQuotedString(string value)
        {
            string[] parts = value.Split('\'');

            StringBuilder builder = new StringBuilder("concat(");

            string prefix = string.Empty;

            string GetCommaOrEmpty()
            {
                if (prefix.Length == 0)
                {
                    prefix = ",";
                    return string.Empty;
                }
                else
                {
                    return prefix;
                }
            }

            bool canAppendQuoteString = false;

            for (int i = 0; i < parts.Length; i++)
            {
                int countOfEmptySiblings = parts.Skip(i).TakeWhile(x => x.Length == 0).Count();

                if (countOfEmptySiblings == 0)
                {
                    if (canAppendQuoteString)
                        builder.AppendFormat("{0}\"'\"", GetCommaOrEmpty());

                    builder.AppendFormat("{0}'{1}'", GetCommaOrEmpty(), parts[i]);
                    canAppendQuoteString = true;
                }

                if (countOfEmptySiblings > 0)
                {
                    var countOfQuote = i > 0 && i + countOfEmptySiblings < parts.Length
                        ? countOfEmptySiblings + 1
                        : countOfEmptySiblings;

                    builder.AppendFormat("{0}\"{1}\"", GetCommaOrEmpty(), new string('\'', countOfQuote));

                    i += countOfEmptySiblings - 1;
                    canAppendQuoteString = false;
                }
            }

            return builder.Append(')').ToString();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string ToString() => Value;
    }
}
