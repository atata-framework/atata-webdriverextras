namespace Atata.WebDriverExtras.Tests;

[TestFixture]
public class ExceptionFactoryTests
{
    [Test]
    public void ExceptionFactory_CreateForNoSuchElement_Null()
    {
        var exception = ExceptionFactory.CreateForNoSuchElement(null);

        string expectedMessage = new SearchFailureData().ToStringForNoSuchElement();

        Assert.That(exception.Message, Does.StartWith(expectedMessage));
    }

    [Test]
    public void ExceptionFactory_CreateForNoSuchElement()
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

    [Test]
    public void ExceptionFactory_CreateForNotMissingElement_Null()
    {
        var exception = ExceptionFactory.CreateForNotMissingElement(null);

        string expectedMessage = new SearchFailureData().ToStringForNotMissingElement();

        Assert.That(exception.Message, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void ExceptionFactory_CreateForNotMissingElement()
    {
        SearchFailureData data = new SearchFailureData
        {
            By = By.XPath(".//a"),
            SearchOptions = SearchOptions.Visible()
        };

        var exception = ExceptionFactory.CreateForNotMissingElement(data);

        string expectedMessage = data.ToStringForNotMissingElement();

        Assert.That(exception.Message, Is.EqualTo(expectedMessage));
    }
}
