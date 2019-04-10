# Atata.WebDriverExtras

[![NuGet](http://img.shields.io/nuget/v/Atata.WebDriverExtras.svg?style=flat)](https://www.nuget.org/packages/Atata.WebDriverExtras/)
[![GitHub release](https://img.shields.io/github/release/atata-framework/atata-webdriverextras.svg)](https://github.com/atata-framework/atata-webdriverextras/releases)
[![Build status](https://dev.azure.com/atata-framework/atata-webdriverextras/_apis/build/status/atata-webdriverextras-ci)](https://dev.azure.com/atata-framework/atata-webdriverextras/_build/latest?definitionId=11)
[![Gitter](https://badges.gitter.im/atata-framework/atata-webdriverextras.svg)](https://gitter.im/atata-framework/atata-webdriverextras)
[![Slack](https://img.shields.io/badge/join-Slack-green.svg?colorB=4EB898)](https://join.slack.com/t/atata-framework/shared_invite/enQtNDMzMzk3OTY5NjgzLTJlNzAyN2E3MzY3MDE4ZGE1ZDQzOGY2NThiYWExZTNkNDc5YjdlNzFjYmUwYjZmNDI2MDJlMGQ3ODNlMDljMzU)
[![Atata docs](https://img.shields.io/badge/docs-Atata_Framework-orange.svg)](https://atata.io)
[![Twitter](https://img.shields.io/badge/follow-@AtataFramework-blue.svg)](https://twitter.com/AtataFramework)

A set of C#/.NET extension methods and other extra classes for Selenium WebDriver. Is a part of [Atata Framework](https://github.com/atata-framework/atata).

Supports .NET Framework 4.0+ and .NET Core/Standard 2.0+.

## Usage

Add `Atata` namespace:

```C#
using Atata;
```

Use extension methods for `IWebDriver`, `IWebElement`, `By`, etc.:

```C#
RemoteWebDriver driver = GetSomeDriver();

// Sets the retry timeout as 7 seconds. The default value of the timeout is 5 seconds.
driver.Manage().Timeouts().SetRetryTimeout(TimeSpan.FromSeconds(7));

// Get the visible element within 7 seconds. Throws NoSuchElementException if the element is not found.
IWebElement element1 = driver.Get(By.Id("some-id"));

// Get the visible element safely (without throw on failure) within 7 seconds. Returns null if the element is not found.
IWebElement element2 = driver.Get(By.XPath(".//some[xpath]").Safely());

// Get all the visible elements within 15 seconds.
ReadOnlyCollection<IWebElement> elements = driver.GetAll(By.ClassName("some-class").Within(TimeSpan.FromSeconds(15)));

// Get the hidden element safely at once (without retry).
IWebElement element3 = driver.Get(By.CssSelector(".some-css").Hidden().Safely().AtOnce());

// Get the element of any visibility unsafely at once.
IWebElement element4 = driver.Get(By.Id("another-id").OfAnyVisibility().AtOnce());

// Gets a value indicating whether the element exists at once.
bool isElementExists = driver.Exists(By.Name("some-name").Safely().AtOnce());

// Waits until the element will be missing within 15 seconds; else throws NotMissingElementException.
driver.Missing(By.Name("some-name").Within(TimeSpan.FromSeconds(15)));

// Get the element using the chain of By.
IWebElement element5 = driver.Get(By.Id("root-container").
    Then(By.XPath("./div[@class='sub-container']")).
    Then(By.CssSelector("span.item")));
```

## License

Atata is an open source software, licensed under the Apache License 2.0.
See [LICENSE](LICENSE) for details.