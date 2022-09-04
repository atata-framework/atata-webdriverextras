using System;
using NUnit.Framework;

namespace Atata.WebDriverExtras.Tests
{
    [TestFixture]
    public class TimeSpanExtensionsTests
    {
        [TestCase(0d, ExpectedResult = "0s")]
        [TestCase(15d, ExpectedResult = "15s")]
        [TestCase(15.0001d, ExpectedResult = "15s")]
        [TestCase(15.12d, ExpectedResult = "15.12s")]
        [TestCase(15.12345d, ExpectedResult = "15.123s")]
        [TestCase(15.1239d, ExpectedResult = "15.123s")]
        [TestCase(60d, ExpectedResult = "1m")]
        [TestCase(60.01d, ExpectedResult = "1m 0.01s")]
        [TestCase(152d, ExpectedResult = "2m 32s")]
        [TestCase(152.9d, ExpectedResult = "2m 32.9s")]
        public string TimeSpanExtensions_ToShortIntervalString(double value) =>
            TimeSpan.FromSeconds(value).ToShortIntervalString();

        [TestCase(0d, ExpectedResult = "0.000s")]
        [TestCase(15d, ExpectedResult = "15.000s")]
        [TestCase(15.0001d, ExpectedResult = "15.000s")]
        [TestCase(15.12d, ExpectedResult = "15.120s")]
        [TestCase(15.12345d, ExpectedResult = "15.123s")]
        [TestCase(15.1239d, ExpectedResult = "15.123s")]
        [TestCase(60d, ExpectedResult = "1m 0.000s")]
        [TestCase(60.01d, ExpectedResult = "1m 0.010s")]
        [TestCase(152d, ExpectedResult = "2m 32.000s")]
        [TestCase(152.9d, ExpectedResult = "2m 32.900s")]
        public string TimeSpanExtensions_ToLongIntervalString(double value) =>
            TimeSpan.FromSeconds(value).ToLongIntervalString();
    }
}
