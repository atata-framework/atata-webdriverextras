namespace Atata.WebDriverExtras.Tests;

public static class ElementExceptionFactoryTests
{
    [TestFixture]
    public class CreateForNotFound
    {
        [Test]
        public void WithNull()
        {
            var result = ElementExceptionFactory.CreateForNotFound(null!);

            string expectedMessage = new SearchFailureData().ToStringForElementNotFound();
            Assert.That(result.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void WithData()
        {
            SearchFailureData data = new SearchFailureData
            {
                By = By.XPath(".//a"),
                SearchOptions = SearchOptions.Visible()
            };

            var result = ElementExceptionFactory.CreateForNotFound(data);

            string expectedMessage = data.ToStringForElementNotFound();
            Assert.That(result.Message, Is.EqualTo(expectedMessage));
        }
    }

    [TestFixture]
    public class CreateForNotMissing
    {
        [Test]
        public void WithNull()
        {
            var result = ElementExceptionFactory.CreateForNotMissing(null!);

            string expectedMessage = new SearchFailureData().ToStringForElementNotMissing();
            Assert.That(result.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void WithData()
        {
            SearchFailureData data = new SearchFailureData
            {
                By = By.XPath(".//a"),
                SearchOptions = SearchOptions.Visible()
            };

            var result = ElementExceptionFactory.CreateForNotMissing(data);

            string expectedMessage = data.ToStringForElementNotMissing();
            Assert.That(result.Message, Is.EqualTo(expectedMessage));
        }
    }
}
