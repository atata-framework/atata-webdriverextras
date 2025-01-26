using System.Collections.ObjectModel;

using AssertIs = NUnit.Framework.Is;

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
            Assert.That(Driver.As<IJavaScriptExecutor>(), AssertIs.EqualTo(Driver));

        [Test]
        public void MissingInterface() =>
            Assert.Throws<NotSupportedException>(() =>
                Driver.As<IEquatable<int>>());

        [Test]
        public void HasInterfaceInWrappedDriver()
        {
            using IWebDriver wrapper = new DriverWrapper(Driver);
            Assert.That(wrapper.As<IJavaScriptExecutor>(), AssertIs.EqualTo(Driver));
        }

        [Test]
        public void MissingInterfaceInWrappedDriver()
        {
            using IWebDriver wrapper = new DriverWrapper(Driver);
            Assert.Throws<NotSupportedException>(() =>
                wrapper.As<IEquatable<int>>());
        }
    }

    public class Is : UITestFixture
    {
        [Test]
        public void WithNull() =>
            Assert.Throws<ArgumentNullException>(() =>
                ((IWebDriver)null).Is<IJavaScriptExecutor>());

        [Test]
        public void HasInterface() =>
            Assert.That(Driver.Is<IJavaScriptExecutor>(), AssertIs.True);

        [Test]
        public void MissingInterface() =>
            Assert.That(Driver.Is<IEquatable<int>>(), AssertIs.False);

        [Test]
        public void HasInterfaceInWrappedDriver()
        {
            using IWebDriver wrapper = new DriverWrapper(Driver);
            Assert.That(wrapper.Is<IJavaScriptExecutor>(), AssertIs.True);
        }

        [Test]
        public void MissingInterfaceInWrappedDriver()
        {
            using IWebDriver wrapper = new DriverWrapper(Driver);
            Assert.That(wrapper.Is<IEquatable<int>>(), AssertIs.False);
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

            Assert.That(result, AssertIs.True);
            Assert.That(casted, AssertIs.EqualTo(Driver));
        }

        [Test]
        public void MissingInterface()
        {
            bool result = Driver.TryAs<IEquatable<int>>(out var casted);

            Assert.That(result, AssertIs.False);
            Assert.That(casted, AssertIs.Null);
        }

        [Test]
        public void HasInterfaceInWrappedDriver()
        {
            using IWebDriver wrapper = new DriverWrapper(Driver);

            bool result = Driver.TryAs<IJavaScriptExecutor>(out var casted);

            Assert.That(result, AssertIs.True);
            Assert.That(casted, AssertIs.EqualTo(Driver));
        }

        [Test]
        public void MissingInterfaceInWrappedDriver()
        {
            using IWebDriver wrapper = new DriverWrapper(Driver);

            bool result = wrapper.TryAs<IEquatable<int>>(out var casted);

            Assert.That(result, AssertIs.False);
            Assert.That(casted, AssertIs.Null);
        }
    }

    private sealed class DriverWrapper : IWebDriver, IWrapsDriver
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
