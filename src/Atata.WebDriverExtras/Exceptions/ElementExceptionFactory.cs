using OpenQA.Selenium;

namespace Atata
{
    public static class ElementExceptionFactory
    {
        public static ElementNotMissingException CreateForNotMissing(string elementName = null, By by = null, ISearchContext searchContext = null) =>
            CreateForNotMissing(
                new SearchFailureData
                {
                    ElementName = elementName,
                    By = by,
                    SearchContext = searchContext
                });

        public static ElementNotMissingException CreateForNotMissing(SearchFailureData searchFailureData)
        {
            string message = (searchFailureData ?? new SearchFailureData()).ToStringForNotMissingElement();

            return new ElementNotMissingException(message);
        }
    }
}
