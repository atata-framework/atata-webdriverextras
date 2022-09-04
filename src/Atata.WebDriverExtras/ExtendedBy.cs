using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace Atata
{
    internal class ExtendedBy : By
    {
        internal ExtendedBy(By by)
        {
            by.CheckNotNull(nameof(by));

            ExtendedBy byAsExtended = by as ExtendedBy;

            By = byAsExtended?.By ?? by;
            Description = By.ToString();

            if (byAsExtended != null)
                ApplySettingsFrom(byAsExtended);
            else
                Options = new SearchOptions();
        }

        internal By By { get; }

        internal string ElementName { get; set; }

        internal string ElementKind { get; set; }

        internal SearchOptions Options { get; set; }

        public ExtendedBy ApplySettingsFrom(ExtendedBy otherExtendedBy)
        {
            otherExtendedBy.CheckNotNull(nameof(otherExtendedBy));

            ElementName = otherExtendedBy.ElementName;
            ElementKind = otherExtendedBy.ElementKind;
            Options = otherExtendedBy.Options.Clone();

            return this;
        }

        public override IWebElement FindElement(ISearchContext context) =>
            By.FindElement(context);

        public override ReadOnlyCollection<IWebElement> FindElements(ISearchContext context) =>
            By.FindElements(context);

        public string GetElementNameWithKind()
        {
            bool hasName = !string.IsNullOrWhiteSpace(ElementName);
            bool hasKind = !string.IsNullOrWhiteSpace(ElementKind);

            if (hasName && hasKind)
                return $"\"{ElementName}\" {ElementKind}";
            else if (hasName)
                return ElementName;
            else if (hasKind)
                return ElementKind;
            else
                return null;
        }

        public override string ToString() =>
            By.ToString();
    }
}
