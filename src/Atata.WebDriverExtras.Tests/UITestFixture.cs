using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Atata.WebDriverExtras.Tests
{
    [TestFixture]
    public abstract class UITestFixture
    {
        private const string BaseUrl = "http://localhost:57439/";

        protected RemoteWebDriver Driver { get; private set; }

        [SetUp]
        public void SetUp()
        {
            Driver = new ChromeDriver();
        }

        [TearDown]
        public void TearDown()
        {
            Driver.Close();
        }

        protected RemoteWebDriver GoTo(string relativeUrl)
        {
            Driver.Navigate().GoToUrl(BaseUrl + relativeUrl);
            return Driver;
        }
    }
}
