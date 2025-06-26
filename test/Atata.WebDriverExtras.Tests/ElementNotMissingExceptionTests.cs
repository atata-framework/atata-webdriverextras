namespace Atata.WebDriverExtras.Tests;

public static class ElementNotMissingExceptionTests
{
    [TestFixture]
    public sealed class Create
    {
        [Test]
        public void WithNull()
        {
            var result = ElementNotMissingException.Create(null!);

            string expectedMessage = new SearchFailureData().ToStringForElementNotMissing();
            Assert.That(result.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void WithData()
        {
            SearchFailureData data = new()
            {
                By = By.XPath(".//a"),
                SearchOptions = SearchOptions.Visible()
            };

            var result = ElementNotMissingException.Create(data);

            string expectedMessage = data.ToStringForElementNotMissing();
            Assert.That(result.Message, Is.EqualTo(expectedMessage));
        }
    }
}
