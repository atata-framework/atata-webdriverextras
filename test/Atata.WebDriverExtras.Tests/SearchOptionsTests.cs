namespace Atata.WebDriverExtras.Tests;

[TestFixture]
public class SearchOptionsTests
{
    [Test]
    public void SearchOptions_ToString()
    {
        SearchOptions options = new SearchOptions
        {
            Visibility = Visibility.Visible,
            IsSafely = false,
            Timeout = TimeSpan.FromSeconds(10.5),
            RetryInterval = TimeSpan.FromMilliseconds(750)
        };

        string expected = $"{{Visibility=Visible, Timeout={options.Timeout.ToShortIntervalString()}, RetryInterval={options.RetryInterval.ToShortIntervalString()}, IsSafely=False}}";

        Assert.That(options.ToString(), Is.EqualTo(expected));
    }
}
