using System.Collections.ObjectModel;

namespace Atata.WebDriverExtras.Tests;

public static class IWebDriverExtensionsTests
{
    public class As : UITestFixture
    {
        [Test]
        public void WithNull() =>
            Assert.Throws<ArgumentNullException>(() =>
                ((IWebDriver)null).As<IJavaScriptExecutor>());

        [Test]
        public void HasInterface() =>
            Assert.That(Driver.As<IJavaScriptExecutor>(), Is.EqualTo(Driver));

        [Test]
        public void MissingInterface() =>
            Assert.Throws<NotSupportedException>(() =>
                Driver.As<IEquatable<int>>());

        [Test]
        public void HasInterfaceInWrappedDriver()
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
    }

    public class TryAs : UITestFixture
    {
        [Test]
        public void WithNull() =>
            Assert.Throws<ArgumentNullException>(() =>
                ((IWebDriver)null).TryAs<IJavaScriptExecutor>(out _));

        [Test]
        public void HasInterface()
        {
            bool result = Driver.TryAs<IJavaScriptExecutor>(out var casted);

            Assert.That(result, Is.True);
            Assert.That(casted, Is.EqualTo(Driver));
        }

        [Test]
        public void MissingInterface()
        {
            bool result = Driver.TryAs<IEquatable<int>>(out var casted);

            Assert.That(result, Is.False);
            Assert.That(casted, Is.Null);
        }

        [Test]
        public void HasInterfaceInWrappedDriver()
        {
            using IWebDriver wrapper = new DriverWrapper(Driver);

            bool result = Driver.TryAs<IJavaScriptExecutor>(out var casted);

            Assert.That(result, Is.True);
            Assert.That(casted, Is.EqualTo(Driver));
        }

        [Test]
        public void MissingInterfaceInWrappedDriver()
        {
            using IWebDriver wrapper = new DriverWrapper(Driver);

            bool result = wrapper.TryAs<IEquatable<int>>(out var casted);

            Assert.That(result, Is.False);
            Assert.That(casted, Is.Null);
        }
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
            // Method intentionally left empty.
        }

        public IWebElement FindElement(By by) => throw new NotSupportedException();

        public ReadOnlyCollection<IWebElement> FindElements(By by) => throw new NotSupportedException();

        public IOptions Manage() => throw new NotSupportedException();

        public INavigation Navigate() => throw new NotSupportedException();

        public void Quit() => throw new NotSupportedException();

        public ITargetLocator SwitchTo() => throw new NotSupportedException();
    }
}
