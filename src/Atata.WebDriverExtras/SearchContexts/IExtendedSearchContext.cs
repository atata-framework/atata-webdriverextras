using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Atata
{
    /// <summary>
    /// Defines the interface that extends <see cref="ISearchContext"/>.
    /// </summary>
    public interface IExtendedSearchContext :
        ISearchContext,
        IFindsById,
        IFindsByName,
        IFindsByTagName,
        IFindsByClassName,
        IFindsByLinkText,
        IFindsByPartialLinkText,
        IFindsByCssSelector,
        IFindsByXPath
    {
        bool Exists(By by);

        bool Missing(By by);
    }
}
