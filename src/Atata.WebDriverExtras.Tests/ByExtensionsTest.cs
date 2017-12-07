using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.WebDriverExtras.Tests
{
    public class ByExtensionsTest : UITestFixture
    {
        private readonly By defaultChain = By.Id("root-container").
            Then(By.XPath("./div[@class='sub-container']")).
            Then(By.CssSelector("span.item"));

        public override void SetUp()
        {
            base.SetUp();

            GoTo("structure");
        }

        [Test]
        public void ByExtensions_Then_GetAll()
        {
            var elements = Driver.GetAll(defaultChain.OfAnyVisibility());

            Assert.That(elements, Has.Count.EqualTo(5));
        }

        [Test]
        public void ByExtensions_Then_GetAll_Visible()
        {
            var elements = Driver.GetAll(defaultChain);

            Assert.That(elements, Has.Count.EqualTo(4));
            Assert.That(elements.Last().Text, Is.EqualTo("Item 5"));
        }

        [Test]
        public void ByExtensions_Then_Get()
        {
            var element = Driver.Get(defaultChain);

            Assert.That(element.Text, Is.EqualTo("Item 1"));
        }

        [Test]
        public void ByExtensions_Then_Get_Hidden()
        {
            var element = Driver.Get(defaultChain.Hidden());

            Assert.That(element.GetAttribute("textContent"), Is.EqualTo("Item 3"));
        }

        [Test]
        public void ByExtensions_Then_Get_FormatWith()
        {
            var element = Driver.Get(By.Id("{0}").
                Then(By.CssSelector("div.sub-container")).
                Then(By.XPath(".//span[.='{1}']")).FormatWith("root-container", "Item 2"));

            Assert.That(element.Text, Is.EqualTo("Item 2"));
        }
    }
}
