using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Atata.WebDriverExtras.Tests
{
    [TestFixture]
    public class SearchFailureDataTests
    {
        [Test]
        public void SearchFailureData_ToStringForNoSuchElement_Empty()
        {
            SearchFailureData data = new SearchFailureData();

            string expected = "Unable to locate element.";

            Assert.That(data.ToStringForNoSuchElement(), Is.EqualTo(expected));
        }

        [Test]
        public void SearchFailureData_ToStringForNotMissingElement_Empty()
        {
            SearchFailureData data = new SearchFailureData();

            string expected = "Able to locate element that should be missing.";

            Assert.That(data.ToStringForNotMissingElement(), Is.EqualTo(expected));
        }

        [Test]
        public void SearchFailureData_ToStringForNoSuchElement_By()
        {
            SearchFailureData data = new SearchFailureData
            {
                By = By.XPath(".//a")
            };

            string expected =
@"Unable to locate element:
- By: XPath "".//a""";

            Assert.That(data.ToStringForNoSuchElement(), Is.EqualTo(expected));
        }

        [Test]
        public void SearchFailureData_ToStringForNoSuchElement_ElementName_By()
        {
            SearchFailureData data = new SearchFailureData
            {
                ElementName = "anchor",
                By = By.XPath(".//a")
            };

            string expected =
@"Unable to locate ""anchor"" element:
- By: XPath "".//a""";

            Assert.That(data.ToStringForNoSuchElement(), Is.EqualTo(expected));
        }

        [Test]
        public void SearchFailureData_ToStringForNotMissingElement_ElementName_By()
        {
            SearchFailureData data = new SearchFailureData
            {
                ElementName = "anchor",
                By = By.XPath(".//a")
            };

            string expected =
@"Able to locate ""anchor"" element that should be missing:
- By: XPath "".//a""";

            Assert.That(data.ToStringForNotMissingElement(), Is.EqualTo(expected));
        }

        [Test]
        public void SearchFailureData_ToStringForNoSuchElement_ByWithElementNameAndKind()
        {
            SearchFailureData data = new SearchFailureData
            {
                By = By.XPath(".//a").Named("Any").OfKind("anchor")
            };

            string expected =
@"Unable to locate ""Any"" anchor element:
- By: XPath "".//a""";

            Assert.That(data.ToStringForNoSuchElement(), Is.EqualTo(expected));
        }

        [Test]
        public void SearchFailureData_ToStringForNoSuchElement_ElementName_By_SearchOptions()
        {
            SearchFailureData data = new SearchFailureData
            {
                ElementName = "anchor",
                By = By.XPath(".//a"),
                SearchOptions = SearchOptions.Visible()
            };

            string expected =
$@"Unable to locate visible ""anchor"" element:
- By: XPath "".//a""
- Search options: {data.SearchOptions}";

            Assert.That(data.ToStringForNoSuchElement(), Is.EqualTo(expected));
        }

        [Test]
        public void SearchFailureData_ToStringForNoSuchElement_ElementName_By_SearchTime_SearchOptions()
        {
            SearchFailureData data = new SearchFailureData
            {
                ElementName = "anchor",
                By = By.XPath(".//a"),
                SearchTime = TimeSpan.FromSeconds(1.5d),
                SearchOptions = SearchOptions.Hidden()
            };

            string expected =
$@"Unable to locate hidden ""anchor"" element:
- By: XPath "".//a""
- Search time: {data.SearchTime.Value.ToShortIntervalString()}
- Search options: {data.SearchOptions}";

            Assert.That(data.ToStringForNoSuchElement(), Is.EqualTo(expected));
        }

        [Test]
        public void SearchFailureData_ToStringForNotMissingElement_ElementName_By_SearchTime_SearchOptions()
        {
            SearchFailureData data = new SearchFailureData
            {
                ElementName = "anchor",
                By = By.XPath(".//a"),
                SearchTime = TimeSpan.FromSeconds(1.5d),
                SearchOptions = SearchOptions.Hidden()
            };

            string expected =
$@"Able to locate hidden ""anchor"" element that should be missing:
- By: XPath "".//a""
- Search time: {data.SearchTime.Value.ToShortIntervalString()}
- Search options: {data.SearchOptions}";

            Assert.That(data.ToStringForNotMissingElement(), Is.EqualTo(expected));
        }

        [Test]
        public void SearchFailureData_ToStringForNoSuchElement_By_AlikeElementsWithInverseVisibility()
        {
            SearchFailureData data = new SearchFailureData
            {
                By = By.XPath(".//a"),
                AlikeElementsWithInverseVisibility = new[] { new RemoteWebElement(null, null), new RemoteWebElement(null, null) }
            };

            string expected =
$@"Unable to locate element:
- By: XPath "".//a""";

            Assert.That(data.ToStringForNoSuchElement(), Is.EqualTo(expected));
        }

        [Test]
        public void SearchFailureData_ToStringForNoSuchElement_By_SearchOptions_AlikeElementsWithInverseVisibility_1()
        {
            SearchFailureData data = new SearchFailureData
            {
                By = By.XPath(".//a"),
                SearchOptions = SearchOptions.Visible(),
                AlikeElementsWithInverseVisibility = new[] { new RemoteWebElement(null, null) }
            };

            string expected =
$@"Unable to locate visible element:
- By: XPath "".//a""
- Search options: {data.SearchOptions}
- Notice: Found 1 element matching specified selector but hidden";

            Assert.That(data.ToStringForNoSuchElement(), Is.EqualTo(expected));
        }

        [Test]
        public void SearchFailureData_ToStringForNoSuchElement_By_SearchOptions_AlikeElementsWithInverseVisibility_2()
        {
            SearchFailureData data = new SearchFailureData
            {
                By = By.XPath(".//a"),
                SearchOptions = SearchOptions.Visible(),
                AlikeElementsWithInverseVisibility = new[] { new RemoteWebElement(null, null), new RemoteWebElement(null, null) }
            };

            string expected =
$@"Unable to locate visible element:
- By: XPath "".//a""
- Search options: {data.SearchOptions}
- Notice: Found 2 elements matching specified selector but hidden";

            Assert.That(data.ToStringForNoSuchElement(), Is.EqualTo(expected));
        }

        [Test]
        public void SearchFailureData_ToStringForNotMissingElement_By_SearchOptions_AlikeElementsWithInverseVisibility_2()
        {
            SearchFailureData data = new SearchFailureData
            {
                By = By.XPath(".//a"),
                SearchOptions = SearchOptions.Visible(),
                AlikeElementsWithInverseVisibility = new[] { new RemoteWebElement(null, null), new RemoteWebElement(null, null) }
            };

            string expected =
$@"Able to locate visible element that should be missing:
- By: XPath "".//a""
- Search options: {data.SearchOptions}";

            Assert.That(data.ToStringForNotMissingElement(), Is.EqualTo(expected));
        }

        [Test]
        public void SearchFailureData_ToStringForNoSuchElement_By_SearchContext()
        {
            IWebElement contextElement = StubWebElement.Div;

            SearchFailureData data = new SearchFailureData
            {
                By = By.XPath(".//a"),
                SearchContext = contextElement
            };

            string expected =
$@"Unable to locate element:
- By: XPath "".//a""

Context element:
{contextElement.ToDetailedString()}";

            Assert.That(data.ToStringForNoSuchElement(), Is.EqualTo(expected));
        }

        [Test]
        public void SearchFailureData_ToStringForNotMissingElement_By_SearchContext()
        {
            IWebElement contextElement = StubWebElement.Div;

            SearchFailureData data = new SearchFailureData
            {
                By = By.XPath(".//a"),
                SearchContext = contextElement
            };

            string expected =
$@"Able to locate element that should be missing:
- By: XPath "".//a""

Context element:
{contextElement.ToDetailedString()}";

            Assert.That(data.ToStringForNotMissingElement(), Is.EqualTo(expected));
        }

        [Test]
        public void SearchFailureData_ToStringForNoSuchElement_SearchContext()
        {
            IWebElement contextElement = StubWebElement.Div;

            SearchFailureData data = new SearchFailureData
            {
                SearchContext = contextElement
            };

            string expected =
$@"Unable to locate element:

Context element:
{contextElement.ToDetailedString()}";

            Assert.That(data.ToStringForNoSuchElement(), Is.EqualTo(expected));
        }
    }
}
