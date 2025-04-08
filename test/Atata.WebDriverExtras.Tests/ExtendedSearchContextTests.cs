namespace Atata.WebDriverExtras.Tests;

[Parallelizable(ParallelScope.None)]
public class ExtendedSearchContextTests : UITestFixture
{
    private readonly By _existingElementBy = By.Id("first-name");

    private readonly By _missingElementBy = By.Id("unknown");

    private readonly By _anotherMissingElementBy = By.XPath("//a[@id='noid']");

    private readonly By _hiddenElementBy = By.Id("hidden-input");

    [Test]
    public void Get_Immediate()
    {
        GoTo("static");

        IWebElement? element;

        using (StopwatchAsserter.WithinSeconds(0, .3))
            element = Driver.Get(_existingElementBy);

        Assert.That(element, Is.Not.Null);
    }

    [Test]
    public void Get_Safely()
    {
        GoTo("static");

        IWebElement? element;

        using (StopwatchAsserter.WithinSeconds(5, .3))
            element = Driver.Get(_missingElementBy.Safely());

        Assert.That(element, Is.Null);
    }

    [Test]
    public void Get_Unsafely()
    {
        GoTo("static");

        using (StopwatchAsserter.WithinSeconds(5, .3))
            Assert.Throws<ElementNotFoundException>(() =>
                Driver.Get(_missingElementBy.Unsafely()));
    }

    [Test]
    public void Get_Hidden()
    {
        GoTo("static");

        using (StopwatchAsserter.WithinSeconds(0, .3))
        {
            IWebElement? element = Driver.Get(_hiddenElementBy.Hidden());
            Assert.That(element, Is.Not.Null);
        }
    }

    [Test]
    public void Get_OfAnyVisibility()
    {
        GoTo("static");

        using (StopwatchAsserter.WithinSeconds(0, .3))
        {
            IWebElement? element = Driver.Get(_hiddenElementBy.OfAnyVisibility());
            Assert.That(element, Is.Not.Null);
        }
    }

    [Test]
    public void Get_Timeout()
    {
        GoTo("static");

        using (StopwatchAsserter.WithinSeconds(3, .3))
            Assert.Throws<ElementNotFoundException>(() =>
                Driver.Get(_missingElementBy.Within(TimeSpan.FromSeconds(3))));
    }

    [Test]
    public void Get_Timeout_ButHidden()
    {
        GoTo("static");

        using (StopwatchAsserter.WithinSeconds(3, .3))
            Assert.Throws<ElementNotFoundException>(() =>
                Driver.Get(_hiddenElementBy.Visible().Within(TimeSpan.FromSeconds(3))));
    }

    [Test]
    public void Get_Retry()
    {
        GoTo("dynamic");

        Driver.Get(By.Id("add-value"))!.Click();

        IWebElement? element = StopwatchAsserter.WithinSeconds(2, 1.5).Execute(
            () => Driver.Get(By.Id("value-block")));

        Assert.That(element, Is.Not.Null);
    }

    [Test]
    public void Try_Get_Timeout()
    {
        GoTo("static");

        using (StopwatchAsserter.WithinSeconds(3, .3))
            Assert.Throws<ElementNotFoundException>(() =>
                Driver.Try(TimeSpan.FromSeconds(3)).Get(_missingElementBy));
    }

    [Test]
    public void Try_Until_Immediate()
    {
        GoTo("static");

        bool result;

        using (StopwatchAsserter.WithinSeconds(0, .05))
            result = Driver.Try(TimeSpan.FromSeconds(2)).Until(x => true);

        Assert.That(result, Is.True);
    }

    [Test]
    public void Try_Until_DefaultTimeout()
    {
        GoTo("static");

        bool result;

        using (StopwatchAsserter.WithinSeconds(5, .3))
            result = Driver.Try().Until(x => false);

        Assert.That(result, Is.False);
    }

    [Test]
    public void Try_Until_TimeoutOfTry()
    {
        GoTo("static");

        bool result;

        using (StopwatchAsserter.WithinSeconds(2, .15))
            result = Driver.Try(TimeSpan.FromSeconds(2)).Until(x => false);

        Assert.That(result, Is.False);
    }

    [Test]
    public void Try_Until_TimeoutOfUntil()
    {
        GoTo("static");

        bool result;

        using (StopwatchAsserter.WithinSeconds(2, .15))
            result = Driver.Try().Until(x => false, TimeSpan.FromSeconds(2));

        Assert.That(result, Is.False);
    }

    [Test]
    public void Missing()
    {
        GoTo("static");

        bool result;

        using (StopwatchAsserter.WithinSeconds(0, .3))
            result = Driver.Try().Missing(_missingElementBy.Safely());

        Assert.That(result, Is.True);
    }

    [Test]
    public void Missing_Timeout_Unsafely()
    {
        GoTo("static");

        using (StopwatchAsserter.WithinSeconds(5, .3))
            Assert.Throws<ElementNotMissingException>(() =>
                Driver.Try().Missing(_existingElementBy.Unsafely()));
    }

    [Test]
    public void Missing_Timeout_Safely()
    {
        GoTo("static");

        bool result;

        using (StopwatchAsserter.WithinSeconds(5, .3))
            result = Driver.Try().Missing(_existingElementBy.Safely());

        Assert.That(result, Is.False);
    }

    [Test]
    public void MissingAll()
    {
        GoTo("static");

        bool result;

        using (StopwatchAsserter.WithinSeconds(0, .6))
            result = Driver.Try().MissingAll(_missingElementBy, _anotherMissingElementBy);

        Assert.That(result, Is.True);
    }

    [Test]
    public void MissingAll_Timeout_Unsafely()
    {
        GoTo("static");

        using (StopwatchAsserter.WithinSeconds(5, .3))
            Assert.Throws<ElementNotMissingException>(() =>
                Driver.Try().MissingAll(_existingElementBy.Unsafely(), _anotherMissingElementBy.Unsafely()));
    }

    [Test]
    public void MissingAll_Timeout_Safely()
    {
        GoTo("static");

        bool result;

        using (StopwatchAsserter.WithinSeconds(5, .5))
            result = Driver.Try().MissingAll(_existingElementBy.Safely(), _anotherMissingElementBy.Safely());

        Assert.That(result, Is.False);
    }
}
