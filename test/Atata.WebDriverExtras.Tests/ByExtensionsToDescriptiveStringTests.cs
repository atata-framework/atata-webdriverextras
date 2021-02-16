using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.WebDriverExtras.Tests
{
    [TestFixture]
    public class ByExtensionsToDescriptiveStringTests
    {
        private static readonly TestCaseData[] Data = new[]
        {
            new TestCaseData(By.Id("some")).Returns(@"id ""some"""),
            new TestCaseData(By.Name("some")).Returns(@"name ""some"""),
            new TestCaseData(By.ClassName("some")).Returns(@"class ""some"""),
            new TestCaseData(By.CssSelector("some")).Returns(@"CSS selector ""some"""),
            new TestCaseData(By.XPath("some")).Returns(@"XPath ""some"""),
            new TestCaseData(By.TagName("some")).Returns(@"tag name ""some"""),
            new TestCaseData(By.LinkText("some")).Returns(@"link text ""some"""),
            new TestCaseData(By.PartialLinkText("some")).Returns(@"partial link text ""some"""),
            new TestCaseData(
                new ByChain(
                    By.Id("some1"),
                    new ByChain(By.Name("some2"), By.ClassName("some3"))))
                .Returns(@"chain [id ""some1"", chain [name ""some2"", class ""some3""]]")
        };

        [TestCaseSource(nameof(Data))]
        public string ByExtensions_ToDescriptiveString(By by)
        {
            return by.ToDescriptiveString();
        }
    }
}
