# Atata.WebDriverExtras

[![NuGet](http://img.shields.io/nuget/v/Atata.WebDriverExtras.svg?style=flat)](https://www.nuget.org/packages/Atata.WebDriverExtras/)
[![GitHub release](https://img.shields.io/github/release/atata-framework/atata-webdriverextras.svg)](https://github.com/atata-framework/atata-webdriverextras/releases)
[![Build status](https://dev.azure.com/atata-framework/atata-webdriverextras/_apis/build/status/atata-webdriverextras-ci?&branchName=main)](https://dev.azure.com/atata-framework/atata-webdriverextras/_build/latest?definitionId=11&branchName=main)
[![Gitter](https://badges.gitter.im/atata-framework/atata-webdriverextras.svg)](https://gitter.im/atata-framework/atata-webdriverextras)
[![Slack](https://img.shields.io/badge/join-Slack-green.svg?colorB=4EB898)](https://join.slack.com/t/atata-framework/shared_invite/zt-5j3lyln7-WD1ZtMDzXBhPm0yXLDBzbA)
[![Atata docs](https://img.shields.io/badge/docs-Atata_Framework-orange.svg)](https://atata.io)
[![Twitter](https://img.shields.io/badge/follow-@AtataFramework-blue.svg)](https://twitter.com/AtataFramework)

A set of C#/.NET extension methods and other extra classes for Selenium WebDriver.
Is a part of [Atata Framework](https://atata.io).

*The package targets .NET Standard 2.0, which supports .NET 5+, .NET Framework 4.6.1+ and .NET Core/Standard 2.0+.*

**[What's new in v3.2.0](https://github.com/atata-framework/atata-webdriverextras/releases/tag/v3.2.0)**

## Usage

Add `Atata` namespace:

```C#
using Atata;
```

Use extension methods for `IWebDriver`, `IWebElement`, `By`, etc.:

```C#
IWebDriver driver = new ChromeDriver();

// Sets the retry timeout as 7 seconds. The default value of the timeout is 5 seconds.
RetrySettings.Timeout = TimeSpan.FromSeconds(7);

// Get the visible element within 7 seconds. Throws ElementNotFoundException if the element is not found.
IWebElement? element1 = driver.Get(By.Id("some-id"));

// Get the visible element safely (without throw on failure) within 7 seconds. Returns null if the element is not found.
IWebElement? element2 = driver.Get(By.XPath(".//some[xpath]").Safely());

// Get all the visible elements within 15 seconds.
ReadOnlyCollection<IWebElement> elements = driver.GetAll(By.ClassName("some-class").Within(TimeSpan.FromSeconds(15)));

// Get the visible element unsafely at once (without retry).
IWebElement? element3 = driver.Get(By.Id("another-id").Visible().AtOnce());

// Get the hidden element safely at once.
IWebElement? element4 = driver.Get(By.CssSelector(".some-css").Hidden().Safely().AtOnce());

// Gets a value indicating whether the element exists at once.
bool isElementExists = driver.Exists(By.Name("some-name").Safely().AtOnce());

// Waits until the element will be missing within 15 seconds; else throws ElementNotMissingException.
driver.Missing(By.Name("some-name").Within(TimeSpan.FromSeconds(15)));

// Get the element using the chain of By.
IWebElement? element5 = driver.Get(By.Id("root-container").
    Then(By.XPath("./div[@class='sub-container']")).
    Then(By.CssSelector("span.item")));

// Set default element visibility for search globally.
SearchOptions.DefaultVisibility = Visibility.Visible;

// After DefaultVisibility is set to Visibility.Visible, the code below will find only visible element.
IWebElement? element6 = driver.Get(By.Id("some-id"));
```

## License

Atata is an open source software, licensed under the Apache License 2.0.
See [LICENSE](LICENSE) for details.
