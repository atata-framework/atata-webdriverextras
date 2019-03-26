using System;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.WebDriverExtras.Tests
{
    [Parallelizable(ParallelScope.None)]
    public class ExtendedSearchContextTests : UITestFixture
    {
        [Test]
        public void ExtendedSearchContext_Get_Immediate()
        {
            GoTo("static");

            IWebElement element;

            using (StopwatchAsserter.Within(.2, .2))
                element = Driver.Get(By.Id("first-name"));

            Assert.That(element, Is.Not.Null);
        }

        [Test]
        public void ExtendedSearchContext_Get_Safely()
        {
            GoTo("static");

            IWebElement element;

            using (StopwatchAsserter.Within(5, .2))
                element = Driver.Get(By.Id("unknown").Safely());

            Assert.That(element, Is.Null);
        }

        [Test]
        public void ExtendedSearchContext_Get_Unsafely()
        {
            GoTo("static");

            using (StopwatchAsserter.Within(5, .2))
                Assert.Throws<NoSuchElementException>(() =>
                    Driver.Get(By.Id("unknown").Unsafely()));
        }

        [Test]
        public void ExtendedSearchContext_Get_Hidden()
        {
            GoTo("static");

            using (StopwatchAsserter.Within(.2, .2))
            {
                IWebElement element = Driver.Get(By.Id("hidden-input").Hidden());
                Assert.That(element, Is.Not.Null);
            }
        }

        [Test]
        public void ExtendedSearchContext_Get_OfAnyVisibility()
        {
            GoTo("static");

            using (StopwatchAsserter.Within(.2, .2))
            {
                IWebElement element = Driver.Get(By.Id("hidden-input").OfAnyVisibility());
                Assert.That(element, Is.Not.Null);
            }
        }

        [Test]
        public void ExtendedSearchContext_Get_Timeout()
        {
            GoTo("static");

            using (StopwatchAsserter.Within(3, .2))
                Assert.Throws<NoSuchElementException>(() =>
                    Driver.Get(By.Id("unknown").Within(TimeSpan.FromSeconds(3))));
        }

        [Test]
        public void ExtendedSearchContext_Get_Timeout_ButHidden()
        {
            GoTo("static");

            using (StopwatchAsserter.Within(3, .2))
                Assert.Throws<NoSuchElementException>(() =>
                    Driver.Get(By.Id("hidden-input").Within(TimeSpan.FromSeconds(3))));
        }

        [Test]
        public void ExtendedSearchContext_Get_Retry()
        {
            GoTo("dynamic");

            Driver.Get(By.Id("add-value")).Click();

            IWebElement element;

            using (StopwatchAsserter.Within(2, .2))
                element = Driver.Get(By.Id("value-block"));

            Assert.That(element, Is.Not.Null);
        }

        [Test]
        public void ExtendedSearchContext_Try_Get_Timeout()
        {
            GoTo("static");

            using (StopwatchAsserter.Within(3, .2))
                Assert.Throws<NoSuchElementException>(() =>
                    Driver.Try(TimeSpan.FromSeconds(3)).Get(By.Id("unknown")));
        }

        [Test]
        public void ExtendedSearchContext_Try_Until_Immediate()
        {
            GoTo("static");

            bool result;

            using (StopwatchAsserter.Within(.05, .05))
                result = Driver.Try(TimeSpan.FromSeconds(2)).Until(x => true);

            Assert.That(result, Is.True);
        }

        [Test]
        public void ExtendedSearchContext_Try_Until_DefaultTimeout()
        {
            GoTo("static");

            bool result;

            using (StopwatchAsserter.Within(5, .1))
                result = Driver.Try().Until(x => false);

            Assert.That(result, Is.False);
        }

        [Test]
        public void ExtendedSearchContext_Try_Until_TimeoutOfTry()
        {
            GoTo("static");

            bool result;

            using (StopwatchAsserter.Within(2, .05))
                result = Driver.Try(TimeSpan.FromSeconds(2)).Until(x => false);

            Assert.That(result, Is.False);
        }

        [Test]
        public void ExtendedSearchContext_Try_Until_TimeoutOfUntil()
        {
            GoTo("static");

            bool result;

            using (StopwatchAsserter.Within(2, .05))
                result = Driver.Try().Until(x => false, TimeSpan.FromSeconds(2));

            Assert.That(result, Is.False);
        }
    }
}
