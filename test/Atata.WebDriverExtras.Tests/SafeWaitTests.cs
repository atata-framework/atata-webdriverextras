namespace Atata.WebDriverExtras.Tests;

[TestFixture]
[Parallelizable(ParallelScope.None)]
public class SafeWaitTests
{
    private SafeWait<object> _sut;

    [SetUp]
    public void SetUp() =>
        _sut = new SafeWait<object>(new object())
        {
            Timeout = TimeSpan.FromSeconds(.3),
            PollingInterval = TimeSpan.FromSeconds(.05)
        };

    [Test]
    public void SafeWait_Success_Immediate()
    {
        using (StopwatchAsserter.WithinSeconds(0, .01))
            _sut.Until(_ => true);
    }

    [Test]
    public void SafeWait_Timeout()
    {
        using (StopwatchAsserter.WithinSeconds(.3, .1))
            _sut.Until(_ => false);
    }

    [Test]
    public void SafeWait_PollingInterval()
    {
        using (StopwatchAsserter.WithinSeconds(.3, .2))
            _sut.Until(_ =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(.1));
                return false;
            });
    }

    [Test]
    public void SafeWait_PollingInterval_GreaterThanTimeout()
    {
        _sut.PollingInterval = TimeSpan.FromSeconds(1);

        using (StopwatchAsserter.WithinSeconds(.3, .1))
            _sut.Until(_ => false);
    }
}
