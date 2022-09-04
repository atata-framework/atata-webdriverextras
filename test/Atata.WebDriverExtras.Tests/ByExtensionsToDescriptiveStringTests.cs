namespace Atata.WebDriverExtras.Tests;

[TestFixture]
public class ByExtensionsToDescriptiveStringTests
{
    private static readonly TestCaseData[] s_data = new[]
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

    [TestCaseSource(nameof(s_data))]
    public string ByExtensions_ToDescriptiveString(By by) =>
        by.ToDescriptiveString();
}
