using System;
using System.Collections.ObjectModel;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.WebDriverExtras.Tests
{
    public static class IWebDriverExtensionsTests
    {
        public class As : UITestFixture
        {
            [Test]
            public void WithNull() =>
                Assert.Throws<ArgumentNullException>(() =>
                    ((IWebDriver)null).As<IJavaScriptExecutor>());

            [Test]
            public void DirectInterface() =>
                Assert.That(Driver.As<IJavaScriptExecutor>(), Is.EqualTo(Driver));

            [Test]
            public void MissingInterface() =>
                Assert.Throws<NotSupportedException>(() =>
                    Driver.As<IEquatable<int>>());

            [Test]
            public void InterfaceInWrappedDriver()
            {
                using IWebDriver wrapper = new DriverWrapper(Driver);
                Assert.That(wrapper.As<IJavaScriptExecutor>(), Is.EqualTo(Driver));
            }

            [Test]
            public void MissingInterfaceInWrappedDriver()
            {
                using IWebDriver wrapper = new DriverWrapper(Driver);
                Assert.Throws<NotSupportedException>(() =>
                    wrapper.As<IEquatable<int>>());
            }

            private class DriverWrapper : IWebDriver, IWrapsDriver
            {
                public DriverWrapper(IWebDriver webDriver) =>
                    WrappedDriver = webDriver;

                public string Url { get; set; }

                public string Title { get; }

                public string PageSource { get; }

                public string CurrentWindowHandle { get; }

                public ReadOnlyCollection<string> WindowHandles { get; }

                public IWebDriver WrappedDriver { get; }

                public void Close() => throw new NotSupportedException();

                public void Dispose()
                {
                }

                public IWebElement FindElement(By by) => throw new NotSupportedException();

                public ReadOnlyCollection<IWebElement> FindElements(By by) => throw new NotSupportedException();

                public IOptions Manage() => throw new NotSupportedException();

                public INavigation Navigate() => throw new NotSupportedException();

                public void Quit() => throw new NotSupportedException();

                public ITargetLocator SwitchTo() => throw new NotSupportedException();
            }
        }
    }
}
