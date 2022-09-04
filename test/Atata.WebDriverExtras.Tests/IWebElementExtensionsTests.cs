namespace Atata.WebDriverExtras.Tests;

public class IWebElementExtensionsTests : UITestFixture
{
    [Test]
    public void IWebElementExtensions_GetElementId()
    {
        GoTo("static");

        IWebElement element = Driver.Get(By.Id("first-name"));

        string id = element.GetElementId();

        Assert.That(id, Is.Not.Null.And.Not.Empty);
    }
}
