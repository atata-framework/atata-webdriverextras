namespace Atata.WebDriverExtras.Tests;

public static class ElementExceptionFactoryTests
{
    [TestFixture]
    public class CreateForNotMissing
    {
        [Test]
        public void WithNull()
        {
            var result = ElementExceptionFactory.CreateForNotMissing(null);

            string expectedMessage = new SearchFailureData().ToStringForNotMissingElement();
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

            string expectedMessage = data.ToStringForNotMissingElement();
            Assert.That(result.Message, Is.EqualTo(expectedMessage));
        }
    }
}
