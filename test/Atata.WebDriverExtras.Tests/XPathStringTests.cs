using NUnit.Framework;

namespace Atata.WebDriverExtras.Tests
{
    [TestFixture]
    public class XPathStringTests
    {
        [TestCase("abc", ExpectedResult = "'abc'")]
        [TestCase("a\"b", ExpectedResult = "'a\"b'")]
        [TestCase("a\"b\"c", ExpectedResult = "'a\"b\"c'")]
        [TestCase("\"a\"b\"c\"", ExpectedResult = "'\"a\"b\"c\"'")]

        [TestCase("a'b", ExpectedResult = "\"a'b\"")]
        [TestCase("a'b'c", ExpectedResult = "\"a'b'c\"")]
        [TestCase("'a'b'c'", ExpectedResult = "\"'a'b'c'\"")]

        [TestCase("a'b\"c", ExpectedResult = "concat('a',\"'\",'b\"c')")]
        [TestCase("a''b\"c", ExpectedResult = "concat('a',\"''\",'b\"c')")]
        [TestCase("a'''b\"c", ExpectedResult = "concat('a',\"'''\",'b\"c')")]

        [TestCase("'a\"b", ExpectedResult = "concat(\"'\",'a\"b')")]
        [TestCase("''a\"b", ExpectedResult = "concat(\"''\",'a\"b')")]
        [TestCase("'''a\"b", ExpectedResult = "concat(\"'''\",'a\"b')")]

        [TestCase("a\"b'", ExpectedResult = "concat('a\"b',\"'\")")]
        [TestCase("a\"b''", ExpectedResult = "concat('a\"b',\"''\")")]
        [TestCase("a\"b'''", ExpectedResult = "concat('a\"b',\"'''\")")]

        [TestCase("'a'b\"c'", ExpectedResult = "concat(\"'\",'a',\"'\",'b\"c',\"'\")")]
        [TestCase("''a'b\"c''", ExpectedResult = "concat(\"''\",'a',\"'\",'b\"c',\"''\")")]
        [TestCase("'''a'''b\"c'''", ExpectedResult = "concat(\"'''\",'a',\"'''\",'b\"c',\"'''\")")]
        [TestCase("''\"''", ExpectedResult = "concat(\"''\",'\"',\"''\")")]
        public string XPathString_ConvertTo(string value) =>
            XPathString.ConvertTo(value);
    }
}
