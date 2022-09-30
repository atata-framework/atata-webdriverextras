using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Defines the interface that extends <see cref="ISearchContext"/>.
    /// </summary>
    public interface IExtendedSearchContext : ISearchContext
    {
        bool Exists(By by);

        bool Missing(By by);
    }
}
