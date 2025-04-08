namespace Atata;

public abstract class XPathBuilder<TBuilder>
    where TBuilder : XPathBuilder<TBuilder>
{
    public string XPath { get; protected set; } = string.Empty;

    public TBuilder Descendant =>
        string.IsNullOrEmpty(XPath)
            ? _(".//")
            : AppendAxis("descendant");

    public TBuilder DescendantOrSelf =>
        AppendAxis("descendant-or-self");

    public TBuilder Child =>
        _("/");

    public TBuilder Self =>
        AppendAxis("self");

    public TBuilder Parent =>
        AppendAxis("parent");

    public TBuilder Following =>
        AppendAxis("following");

    public TBuilder FollowingSibling =>
        AppendAxis("following-sibling");

    public TBuilder Ancestor =>
        AppendAxis("ancestor");

    public TBuilder AncestorOrSelf =>
        AppendAxis("ancestor-or-self");

    public TBuilder Preceding =>
        AppendAxis("preceding");

    public TBuilder PrecedingSibling =>
        AppendAxis("preceding-sibling");

    public TBuilder Any =>
        _("*");

    public TBuilder Or =>
        _(" or ");

    public TBuilder And =>
        _(" and ");

    public TBuilder this[Func<TBuilder, string> condition] =>
        Where(condition);

    public TBuilder this[object condition] =>
        Where(condition);

    protected TBuilder AppendAxis(string axisName)
    {
        string xPath = axisName + "::";

        if (!string.IsNullOrEmpty(XPath))
        {
            char lastChar = XPath.Last();

            if (!new[] { '[', '(', ' ' }.Contains(lastChar))
                return _($"/{xPath}");
        }

        return _(xPath);
    }

#pragma warning disable S100, SA1300 // Methods and properties should be named in camel case
    public TBuilder _(string xPath)
#pragma warning restore S100, SA1300 // Methods and properties should be named in camel case
    {
        TBuilder newBuidler = CreateInstance();
        newBuidler.XPath = XPath + xPath;

        return newBuidler;
    }

    public TBuilder Class(params string[] classNames) =>
        classNames?.Length > 0
            ? JoinAnd(classNames.Select(x => $"contains(concat(' ', normalize-space(@class), ' '), ' {x} ')"))
            : (TBuilder)this;

    public TBuilder Where(Func<TBuilder, string> condition)
    {
        string subPath = CreateSubPath(condition);
        return _($"[{subPath}]");
    }

    public TBuilder Where(object condition) =>
        _($"[{condition}]");

    public TBuilder WherePosition(int position) =>
        _($"[{position}]");

    public TBuilder WhereIndex(int index) =>
        _($"[{index + 1}]");

    public TBuilder WhereClass(params string[] classNames) =>
        classNames?.Length > 0
            ? _($"[{CreateInstance().Class(classNames).XPath}]")
            : (TBuilder)this;

    public TBuilder Wrap(Func<TBuilder, string> buildAction)
    {
        string subPath = CreateSubPath(buildAction);
        return _($"({subPath})");
    }

    public TBuilder WrapWithIndex(int index, Func<TBuilder, string> buildFunction)
    {
        string subPath = CreateSubPath(buildFunction);

        return _($"({subPath})[{index + 1}]");
    }

    public TBuilder WrapWithPosition(int position, Func<TBuilder, string> buildFunction)
    {
        string subPath = CreateSubPath(buildFunction);

        return _($"({subPath})[{position}]");
    }

    public TBuilder JoinOr(IEnumerable<string> conditions) =>
        _(string.Join(" or ", conditions));

    public TBuilder JoinAnd(IEnumerable<string> conditions) =>
        _(string.Join(" and ", conditions));

    protected abstract TBuilder CreateInstance();

    protected string CreateSubPath(Func<TBuilder, string> buildFunction)
    {
        TBuilder subBuilder = CreateInstance();
        return buildFunction(subBuilder);
    }

    public override string ToString() =>
        XPath;
}
