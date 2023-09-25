namespace Atata.WebDriverExtras.Tests;

[TestFixture]
public class ExceptionFactoryTests
{
    [Test]
    public void CreateForNoSuchElement_Null()
    {
        var exception = ExceptionFactory.CreateForNoSuchElement(null);

        string expectedMessage = new SearchFailureData().ToStringForNoSuchElement();

        Assert.That(exception.Message, Does.StartWith(expectedMessage));
    }

    [Test]
    public void CreateForNoSuchElement()
    {
        SearchFailureData data = new SearchFailureData
        {
            By = By.XPath(".//a"),
            SearchOptions = SearchOptions.Visible()
        };

        var exception = ExceptionFactory.CreateForNoSuchElement(data);

        string expectedMessage = data.ToStringForNoSuchElement();

        Assert.That(exception.Message, Does.StartWith(expectedMessage));
    }
}
