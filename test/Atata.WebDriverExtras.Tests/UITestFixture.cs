using OpenQA.Selenium.Chrome;

namespace Atata.WebDriverExtras.Tests;

[TestFixture]
public abstract class UITestFixture
{
    public const int TestAppPort = 57440;

    public static string BaseUrl { get; } = $"http://localhost:{TestAppPort}/";

    protected IWebDriver Driver { get; private set; }

    [SetUp]
    public virtual void SetUp() =>
        Driver = new ChromeDriver();

    [TearDown]
    public void TearDown()
    {
        Driver?.Close();
        Driver?.Dispose();
    }

    protected IWebDriver GoTo(string relativeUrl)
    {
        Driver.Navigate().GoToUrl(BaseUrl + relativeUrl);
        return Driver;
    }
}
