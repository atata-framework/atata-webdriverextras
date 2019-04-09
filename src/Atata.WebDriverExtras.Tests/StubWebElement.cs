using System;
using System.Collections.ObjectModel;
using System.Drawing;
using OpenQA.Selenium;

namespace Atata.WebDriverExtras.Tests
{
    public class StubWebElement : IWebElement
    {
        public static StubWebElement Div { get; } = new StubWebElement
        {
            Id = "div-id-001",
            TagName = "div",
            Text = "Some text",
            Enabled = true,
            Location = new Point(50, 75),
            Size = new Size(100, 150),
            Displayed = true
        };

        public string Id { get; set; }

        public string TagName { get; set; }

        public string Text { get; set; }

        public bool Enabled { get; set; }

        public bool Selected { get; set; }

        public Point Location { get; set; }

        public Size Size { get; set; }

        public bool Displayed { get; set; }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Click()
        {
            throw new NotImplementedException();
        }

        public IWebElement FindElement(By by)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            throw new NotImplementedException();
        }

        public string GetAttribute(string attributeName)
        {
            throw new NotImplementedException();
        }

        public string GetCssValue(string propertyName)
        {
            throw new NotImplementedException();
        }

        public string GetProperty(string propertyName)
        {
            throw new NotImplementedException();
        }

        public void SendKeys(string text)
        {
            throw new NotImplementedException();
        }

        public void Submit()
        {
            throw new NotImplementedException();
        }
    }
}
