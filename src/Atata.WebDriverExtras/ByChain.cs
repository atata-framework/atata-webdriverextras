namespace Atata;

public class ByChain : By
{
    public ByChain(params By[] items)
        : this(items as IEnumerable<By>)
    {
    }

    public ByChain(IEnumerable<By> items)
    {
        Items = items.ToList().AsReadOnly();
        Description = $"By.Chain([{string.Join(", ", Items)}])";
    }

    public ReadOnlyCollection<By> Items { get; }

    public override IWebElement FindElement(ISearchContext context)
    {
        ReadOnlyCollection<IWebElement> elements = FindElements(context);

        return elements.FirstOrDefault()
            ?? throw ElementExceptionFactory.CreateForNotFound(by: this, searchContext: context);
    }

    public override ReadOnlyCollection<IWebElement> FindElements(ISearchContext context)
    {
        if (Items.Count == 0)
            return new List<IWebElement>().AsReadOnly();

        List<IWebElement> elements = [.. Items[0].FindElements(context)];

        foreach (By by in Items.Skip(1))
        {
            elements = [.. elements.SelectMany(x => x.FindElements(by))];
        }

        return elements.AsReadOnly();
    }
}
