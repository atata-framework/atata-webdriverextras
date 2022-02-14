using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Atata.WebDriverExtras.Tests
{
    [TestFixture]
    public abstract class UITestFixture
    {
        public const string BaseUrl = "http://localhost:57440/";

        protected IWebDriver Driver { get; private set; }

        [SetUp]
        public virtual void SetUp()
        {
            Driver = new ChromeDriver();
        }

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
}
