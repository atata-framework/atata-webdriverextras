# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Changed

- Change package target frameworks from .NET Standard 2.0 to .NET 8.0 and .NET Framework 4.6.2.
- Make obsolete `ElementExceptionFactory.CreateForNotFound` methods.
  Use `ElementNotFoundException.Create(...)` instead.
- Make obsolete `ElementExceptionFactory.CreateForNotMissing` methods.
  Use `ElementNotMissingException.Create(...)` instead.
- Make obsolete `StringBuilderExtensions.AppendSpace` method.
  Use `Append(' ')` instead.
- Make obsolete `IEnumerableExtensions.ToReadOnly` method.
  Instead use constructor of `ReadOnlyCollection<T>` or use another collection type (array, for example).
- Make obsolete `ExceptionFactory.CreateForTimeout` method.
  Use `TimeoutExceptionFactory.Create(...)` instead.
- Make obsolete `ExceptionFactory.CreateForUnsupportedEnumValue` method.
- Change `RetrySettings.DefaultInterval` value from 500 to 200 milliseconds.

### Removed

- Remove obsolete `ITimeoutsExtensions` class.

## [3.5.0] - 2026-01-20

### Changed

- Upgrade Selenium.WebDriver package reference to v4.40.0.

## [3.4.0] - 2025-12-10

### Changed

- Upgrade Selenium.WebDriver package reference to v4.39.0.

## [3.3.0] - 2025-10-13

### Changed

- Upgrade Selenium.WebDriver package reference to v4.36.0.
- Add handling of `OpenQA.Selenium.UnknownErrorException` to `StaleSafely.Execute` methods.

## [3.2.0] - 2025-04-08

### Changed

- Enable nullable reference types.
- Upgrade Selenium.WebDriver package reference to v4.31.0.

## [3.1.0] - 2025-01-26

### Changed

- Upgrade Selenium.WebDriver package reference to v4.28.0 (#61).

[Unreleased]: https://github.com/atata-framework/atata-webdriverextras/compare/v3.5.0...HEAD
[3.5.0]: https://github.com/atata-framework/atata-webdriverextras/compare/v3.4.0...v3.5.0
[3.4.0]: https://github.com/atata-framework/atata-webdriverextras/compare/v3.3.0...v3.4.0
[3.3.0]: https://github.com/atata-framework/atata-webdriverextras/compare/v3.2.0...v3.3.0
[3.2.0]: https://github.com/atata-framework/atata-webdriverextras/compare/v3.1.0...v3.2.0
[3.1.0]: https://github.com/atata-framework/atata-webdriverextras/compare/v3.0.0...v3.1.0
