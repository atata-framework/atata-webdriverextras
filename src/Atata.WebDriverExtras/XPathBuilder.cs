namespace Atata;

public class XPathBuilder : XPathBuilder<XPathBuilder>
{
    public static implicit operator string(XPathBuilder builder) =>
        builder.XPath;

    protected override XPathBuilder CreateInstance() =>
        new();
}
