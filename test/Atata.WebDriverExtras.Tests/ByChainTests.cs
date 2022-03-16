using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.WebDriverExtras.Tests
{
    public class ByChainTests : UITestFixture
    {
        private readonly ByChain _defaultChain = new ByChain(
            By.Id("root-container"),
            By.XPath("./div[@class='sub-container']"),
            By.CssSelector("span.item"));

        public override void SetUp()
        {
            base.SetUp();

            GoTo("structure");
        }

        [Test]
        public void ByChain_GetAll()
        {
            var elements = Driver.GetAll(_defaultChain);

            Assert.That(elements, Has.Count.EqualTo(5));
        }

        [Test]
        public void ByChain_GetAll_Visible()
        {
            var elements = Driver.GetAll(_defaultChain.Visible());

            Assert.That(elements, Has.Count.EqualTo(4));
            Assert.That(elements.Last().Text, Is.EqualTo("Item 5"));
        }

        [Test]
        public void ByChain_Get()
        {
            var element = Driver.Get(_defaultChain);

            Assert.That(element.Text, Is.EqualTo("Item 1"));
        }

        [Test]
        public void ByChain_Get_Hidden()
        {
            var element = Driver.Get(_defaultChain.Hidden());

            Assert.That(element.GetAttribute("textContent"), Is.EqualTo("Item 3"));
        }

        [Test]
        public void ByChain_Get_FormatWith()
        {
            var element = Driver.Get(new ByChain(
                By.Id("{0}"),
                By.CssSelector("div.sub-container"),
                By.XPath(".//span[.='{1}']")).FormatWith("root-container", "Item 2"));

            Assert.That(element.Text, Is.EqualTo("Item 2"));
        }
    }
}
