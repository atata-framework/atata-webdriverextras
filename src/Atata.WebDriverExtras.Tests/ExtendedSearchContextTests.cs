using System;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.WebDriverExtras.Tests
{
    [Parallelizable(ParallelScope.None)]
    public class ExtendedSearchContextTests : UITestFixture
    {
        private readonly By existingElementBy = By.Id("first-name");

        private readonly By missingElementBy = By.Id("unknown");

        private readonly By hiddenElementBy = By.Id("hidden-input");

        [Test]
        public void ExtendedSearchContext_Get_Immediate()
        {
            GoTo("static");

            IWebElement element;

            using (StopwatchAsserter.Within(.2, .2))
                element = Driver.Get(existingElementBy);

            Assert.That(element, Is.Not.Null);
        }

        [Test]
        public void ExtendedSearchContext_Get_Safely()
        {
            GoTo("static");

            IWebElement element;

            using (StopwatchAsserter.Within(5, .2))
                element = Driver.Get(missingElementBy.Safely());

            Assert.That(element, Is.Null);
        }

        [Test]
        public void ExtendedSearchContext_Get_Unsafely()
        {
            GoTo("static");

            using (StopwatchAsserter.Within(5, .2))
                Assert.Throws<NoSuchElementException>(() =>
                    Driver.Get(missingElementBy.Unsafely()));
        }

        [Test]
        public void ExtendedSearchContext_Get_Hidden()
        {
            GoTo("static");

            using (StopwatchAsserter.Within(.2, .2))
            {
                IWebElement element = Driver.Get(hiddenElementBy.Hidden());
                Assert.That(element, Is.Not.Null);
            }
        }

        [Test]
        public void ExtendedSearchContext_Get_OfAnyVisibility()
        {
            GoTo("static");

            using (StopwatchAsserter.Within(.2, .2))
            {
                IWebElement element = Driver.Get(hiddenElementBy.OfAnyVisibility());
                Assert.That(element, Is.Not.Null);
            }
        }

        [Test]
        public void ExtendedSearchContext_Get_Timeout()
        {
            GoTo("static");

            using (StopwatchAsserter.Within(3, .2))
                Assert.Throws<NoSuchElementException>(() =>
                    Driver.Get(missingElementBy.Within(TimeSpan.FromSeconds(3))));
        }

        [Test]
        public void ExtendedSearchContext_Get_Timeout_ButHidden()
        {
            GoTo("static");

            using (StopwatchAsserter.Within(3, .2))
                Assert.Throws<NoSuchElementException>(() =>
                    Driver.Get(hiddenElementBy.Within(TimeSpan.FromSeconds(3))));
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
                    Driver.Try(TimeSpan.FromSeconds(3)).Get(missingElementBy));
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

            using (StopwatchAsserter.Within(5, .2))
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

        [Test]
        public void ExtendedSearchContext_Missing()
        {
            GoTo("static");

            bool result;

            using (StopwatchAsserter.Within(.2, .2))
                result = Driver.Try().Missing(missingElementBy.Safely());

            Assert.That(result, Is.True);
        }

        [Test]
        public void ExtendedSearchContext_Missing_Timeout_Unsafely()
        {
            GoTo("static");

            using (StopwatchAsserter.Within(5, .2))
                Assert.Throws<NotMissingElementException>(() =>
                    Driver.Try().Missing(existingElementBy.Unsafely()));
        }

        [Test]
        public void ExtendedSearchContext_Missing_Timeout_Safely()
        {
            GoTo("static");

            bool result;

            using (StopwatchAsserter.Within(5, .2))
                result = Driver.Try().Missing(existingElementBy.Safely());

            Assert.That(result, Is.False);
        }
    }
}
