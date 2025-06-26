namespace Atata.WebDriverExtras.Tests;

public static class ElementNotFoundExceptionTests
{
    [TestFixture]
    public sealed class Create
    {
        [Test]
        public void WithNull()
        {
            var result = ElementNotFoundException.Create(null!);

            string expectedMessage = new SearchFailureData().ToStringForElementNotFound();
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

            var result = ElementNotFoundException.Create(data);

            string expectedMessage = data.ToStringForElementNotFound();
            Assert.That(result.Message, Is.EqualTo(expectedMessage));
        }
    }
}
