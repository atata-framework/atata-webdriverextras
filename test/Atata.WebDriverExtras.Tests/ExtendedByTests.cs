namespace Atata.WebDriverExtras.Tests;

public sealed class ExtendedByTests
{
    public static IEnumerable<By> ByTestCases =>
        [
            By.Id("some-id"),
            By.CssSelector("some.class"),
            By.XPath("//path")
        ];

    [TestCaseSource(nameof(ByTestCases))]
    public void MechanismAndCriteria(By by)
    {
        ExtendedBy sut = new(by);

        Assert.Multiple(() =>
        {
            Assert.That(sut.Mechanism, Is.EqualTo(by.Mechanism));
            Assert.That(sut.Criteria, Is.EqualTo(by.Criteria));
        });
    }
}
