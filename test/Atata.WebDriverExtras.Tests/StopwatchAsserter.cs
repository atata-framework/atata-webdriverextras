namespace Atata.WebDriverExtras.Tests;

public sealed class StopwatchAsserter : IDisposable
{
    private readonly TimeSpan _expectedTime;

    private readonly TimeSpan _upperToleranceTime;

    private readonly TimeSpan _lowerToleranceTime;

    private readonly Stopwatch _watch;

    private bool _doAssertOnDispose = true;

    public StopwatchAsserter(TimeSpan expectedTime, TimeSpan upperToleranceTime)
        : this(expectedTime, upperToleranceTime, TimeSpan.Zero)
    {
    }

    public StopwatchAsserter(TimeSpan expectedTime, TimeSpan upperToleranceTime, TimeSpan lowerToleranceTime)
    {
        _expectedTime = expectedTime;
        _upperToleranceTime = upperToleranceTime;
        _lowerToleranceTime = lowerToleranceTime;

        _watch = Stopwatch.StartNew();
    }

    public static StopwatchAsserter WithinSeconds(double seconds, double upperToleranceSeconds = 1, double lowerToleranceSeconds = 0.001) =>
        new(
            TimeSpan.FromSeconds(seconds),
            TimeSpan.FromSeconds(upperToleranceSeconds),
            TimeSpan.FromSeconds(lowerToleranceSeconds));

    public TResult Execute<TResult>(Func<TResult> function)
    {
        try
        {
            return function.Invoke();
        }
        catch (Exception)
        {
            _doAssertOnDispose = false;
            throw;
        }
        finally
        {
            Dispose();
        }
    }

    public void Dispose()
    {
        _watch.Stop();

        if (_doAssertOnDispose)
            Assert.That(_watch.Elapsed, Is.InRange(_expectedTime - _lowerToleranceTime, _expectedTime + _upperToleranceTime));
    }
}
