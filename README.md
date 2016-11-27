# Atata.WebDriverExtras

[![NuGet](http://img.shields.io/nuget/v/Atata.WebDriverExtras.svg?style=flat)](https://www.nuget.org/packages/Atata.WebDriverExtras/)
[![Join the chat at https://gitter.im/atata-framework/atata-webdriverextras](https://badges.gitter.im/atata-framework/atata-webdriverextras.svg)](https://gitter.im/atata-framework/atata-webdriverextras?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

A set of C#/.NET extension methods and other extra classes for Selenium WebDriver. Is a part of [Atata Framework](https://github.com/atata-framework/atata).

## Usage

```C#
RemoteWebDriver driver = GetSomeDriver();

// Sets the retry timeout as 5 seconds. The default value of the timeout is 10 seconds.
driver.Manage().Timeouts().SetRetryTimeout(TimeSpan.FromSeconds(5));

// Get the visible element within 5 seconds. Throws NoSuchElementException if the element is not found.
IWebElement element1 = driver.Get(By.Id("some-id"));

// Get the visible element safely (without thow on failure) within 5 seconds. Returns null if the element is not found.
IWebElement element2 = driver.Get(By.XPath(".//some[xpath]").Safely());

// Get all the visible elements within 8 seconds.
ReadOnlyCollection<IWebElement> elements = driver.GetAll(By.ClassName("some-class").Within(TimeSpan.FromSeconds(8)));

// Get the hidden element safely at once (without retry).
IWebElement element3 = driver.Get(By.CssSelector(".some-css").Hidden().Safely().AtOnce());

// Get the element of any visibility unsafely at once.
IWebElement element4 = driver.Get(By.Id("another-id").OfAnyVisibility().AtOnce());

// Gets a value indicating whether the element exists at once.
bool isElementExists = driver.Exists(By.Name("some-name").Safely().AtOnce());

// Waits until the element will be missing within 15 seconds; else throws NotMissingElementException.
driver.Missing(By.Name("some-name").Within(TimeSpan.FromSeconds(15)));
```

Don't forget to add `Atata` namespace:

```C#
using Atata;
```