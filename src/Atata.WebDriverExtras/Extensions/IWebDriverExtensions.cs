using System;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.Interactions;

namespace Atata
{
    public static class IWebDriverExtensions
    {
        /// <summary>
        /// Casts the web driver to <see cref="IJavaScriptExecutor"/> type.
        /// Considers <see cref="IWrapsDriver"/>.
        /// </summary>
        /// <param name="webDriver">The <see cref="IWebDriver"/> instance.</param>
        /// <returns><paramref name="webDriver"/> casted to <see cref="IJavaScriptExecutor"/>.</returns>
        /// <exception cref="NotSupportedException"><paramref name="webDriver"/> doesn't implement <see cref="IJavaScriptExecutor"/>.</exception>
        public static IJavaScriptExecutor AsScriptExecutor(this IWebDriver webDriver) =>
            webDriver.As<IJavaScriptExecutor>();

        /// <summary>
        /// Casts the web driver to <see cref="ITakesScreenshot"/> type.
        /// Considers <see cref="IWrapsDriver"/>.
        /// </summary>
        /// <param name="webDriver">The <see cref="IWebDriver"/> instance.</param>
        /// <returns><paramref name="webDriver"/> casted to <see cref="ITakesScreenshot"/>.</returns>
        /// <exception cref="NotSupportedException"><paramref name="webDriver"/> doesn't implement <see cref="ITakesScreenshot"/>.</exception>
        public static ITakesScreenshot AsScreenshotTaker(this IWebDriver webDriver) =>
            webDriver.As<ITakesScreenshot>();

        /// <summary>
        /// Casts the web driver to <see cref="IDevTools"/> type.
        /// Considers <see cref="IWrapsDriver"/>.
        /// </summary>
        /// <param name="webDriver">The <see cref="IWebDriver"/> instance.</param>
        /// <returns><paramref name="webDriver"/> casted to <see cref="IDevTools"/>.</returns>
        /// <exception cref="NotSupportedException"><paramref name="webDriver"/> doesn't implement <see cref="IDevTools"/>.</exception>
        public static IDevTools AsDevTools(this IWebDriver webDriver) =>
            webDriver.As<IDevTools>();

        /// <summary>
        /// Casts the web driver to the specified interface type.
        /// Considers <see cref="IWrapsDriver"/>.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="webDriver">The <see cref="IWebDriver"/> instance.</param>
        /// <returns><paramref name="webDriver"/> casted to <typeparamref name="TInterface"/>.</returns>
        /// <exception cref="NotSupportedException"><paramref name="webDriver"/> doesn't implement <typeparamref name="TInterface"/>.</exception>
        public static TInterface As<TInterface>(this IWebDriver webDriver) =>
            webDriver.TryAs(out TInterface castedWebDriver)
                ? castedWebDriver
                : throw new NotSupportedException($"{webDriver.GetType().FullName} doesn't implement {typeof(TInterface).FullName}.");

        /// <summary>
        /// Determines whether the web driver implements <typeparamref name="TInterface"/>.
        /// Considers <see cref="IWrapsDriver"/>.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="webDriver">The <see cref="IWebDriver"/> instance.</param>
        /// <returns>
        ///   <c>true</c> if [is] [the specified web driver]; otherwise, <c>false</c>.
        /// </returns>
        public static bool Is<TInterface>(this IWebDriver webDriver) =>
            webDriver.TryAs<TInterface>(out _);

        /// <summary>
        /// Tries to cast the web driver to the specified interface type.
        /// Considers <see cref="IWrapsDriver"/>.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="webDriver">The <see cref="IWebDriver"/> instance.</param>
        /// <param name="castedWebDriver">The casted web driver.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="webDriver"/> can be casted to <typeparamref name="TInterface"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryAs<TInterface>(this IWebDriver webDriver, out TInterface castedWebDriver)
        {
            if (webDriver is null)
                throw new ArgumentNullException(nameof(webDriver));

            if (webDriver is TInterface casted)
            {
                castedWebDriver = casted;
                return true;
            }
            else if (webDriver is IWrapsDriver webDriverWrapper)
            {
                return webDriverWrapper.WrappedDriver.TryAs(out castedWebDriver);
            }
            else
            {
                castedWebDriver = default;
                return false;
            }
        }

        public static T Maximize<T>(this T driver)
            where T : IWebDriver
        {
            driver.CheckNotNull(nameof(driver));

            driver.Manage().Window.Maximize();
            return driver;
        }

        public static T SetSize<T>(this T driver, int width, int height)
            where T : IWebDriver
        {
            driver.CheckNotNull(nameof(driver));

            driver.Manage().Window.Size = new Size(width, height);
            return driver;
        }

        public static T SetPosition<T>(this T driver, int x, int y)
            where T : IWebDriver
        {
            driver.CheckNotNull(nameof(driver));

            driver.Manage().Window.Position = new Point(x, y);
            return driver;
        }

        public static T Perform<T>(this T driver, Func<Actions, Actions> actionsBuilder)
            where T : IWebDriver
        {
            driver.CheckNotNull(nameof(driver));
            actionsBuilder.CheckNotNull(nameof(actionsBuilder));

            Actions actions = new Actions(driver);
            actions = actionsBuilder(actions);
            actions.Perform();

            return driver;
        }

        public static WebDriverExtendedSearchContext Try(this IWebDriver driver)
        {
            driver.CheckNotNull(nameof(driver));

            return new WebDriverExtendedSearchContext(driver);
        }

        public static WebDriverExtendedSearchContext Try(this IWebDriver driver, TimeSpan timeout)
        {
            driver.CheckNotNull(nameof(driver));

            return new WebDriverExtendedSearchContext(driver, timeout);
        }

        public static WebDriverExtendedSearchContext Try(this IWebDriver driver, TimeSpan timeout, TimeSpan retryInterval)
        {
            driver.CheckNotNull(nameof(driver));

            return new WebDriverExtendedSearchContext(driver, timeout, retryInterval);
        }

        public static bool TitleContains(this IWebDriver driver, string text)
        {
            driver.CheckNotNull(nameof(driver));
            text.CheckNotNullOrEmpty(nameof(text));

            return driver?.Title.Contains(text) ?? false;
        }
    }
}
