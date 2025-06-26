namespace Atata;

/// <summary>
/// Represents the data describing element search failure.
/// </summary>
public class SearchFailureData
{
    /// <summary>
    /// Gets or sets the name of the element.
    /// </summary>
    public string? ElementName { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="By"/> instance used for element search.
    /// </summary>
    public By? By { get; set; }

    /// <summary>
    /// Gets or sets the search time.
    /// </summary>
    public TimeSpan? SearchTime { get; set; }

    /// <summary>
    /// Gets or sets the options used during the search.
    /// </summary>
    public SearchOptions? SearchOptions { get; set; }

    /// <summary>
    /// Gets or sets the alike elements with inverse visibility.
    /// </summary>
    public IEnumerable<IWebElement>? AlikeElementsWithInverseVisibility { get; set; }

    /// <summary>
    /// Gets or sets the search context where the element was searched in.
    /// </summary>
    public ISearchContext? SearchContext { get; set; }

    internal string ToStringForElementNotFound()
    {
        StringBuilder builder = new();

        Populate(builder, appendAlikeElementsWithInverseVisibility: true);

        builder.Insert(0, $"Unable to locate {GetFullElementName()}{(builder.Length > 0 ? ":" : ".")}");

        return builder.ToString();
    }

    internal string ToStringForElementNotMissing()
    {
        StringBuilder builder = new();

        Populate(builder, appendAlikeElementsWithInverseVisibility: false);

        builder.Insert(0, $"Able to locate {GetFullElementName()} that should be missing{(builder.Length > 0 ? ":" : ".")}");

        return builder.ToString();
    }

    private string GetFullElementName()
    {
        StringBuilder builder = new();

        if (SearchOptions is not null && SearchOptions.Visibility != Visibility.Any)
            builder.Append(SearchOptions.Visibility.ToString().ToLowerInvariant()).Append(' ');

        string? elementName = GetElementNameOrExtractFromBy();

        if (elementName?.Length > 0)
            builder.Append(WrapWithDoubleQuotes(elementName)).Append(' ');

        return builder.Append("element").ToString();
    }

    private static string WrapWithDoubleQuotes(string name) =>
        name[0] is not '"' and not '\''
            ? $"\"{name}\""
            : name;

    private string? GetElementNameOrExtractFromBy() =>
        ElementName ?? (By as ExtendedBy)?.GetElementNameWithKind();

    private void Populate(StringBuilder builder, bool appendAlikeElementsWithInverseVisibility)
    {
        if (By is not null)
            builder.AppendLine().AppendFormat("- By: {0}", By.ToDescriptiveString());

        if (SearchTime is not null)
            builder.AppendLine().AppendFormat("- Search time: {0}", SearchTime.Value.ToShortIntervalString());

        if (SearchOptions is not null)
            builder.AppendLine().AppendFormat("- Search options: {0}", SearchOptions);

        if (appendAlikeElementsWithInverseVisibility)
            AppendAlikeElementsWithInverseVisibility(builder);

        AppendSearchContext(builder);
    }

    private void AppendAlikeElementsWithInverseVisibility(StringBuilder builder)
    {
        int alikeElementsWithInverseVisibilityCount = AlikeElementsWithInverseVisibility?.Count() ?? 0;

        if (alikeElementsWithInverseVisibilityCount > 0 && SearchOptions is not null && SearchOptions.Visibility != Visibility.Any)
        {
            Visibility inverseVisibility = SearchOptions.Visibility == Visibility.Visible
                ? Visibility.Hidden
                : Visibility.Visible;

            builder.AppendLine().AppendFormat(
                "- Notice: Found {0} element{1} matching specified selector but {2}",
                alikeElementsWithInverseVisibilityCount,
                alikeElementsWithInverseVisibilityCount > 1 ? "s" : null,
                inverseVisibility.ToString().ToLowerInvariant());
        }
    }

    private void AppendSearchContext(StringBuilder builder)
    {
        if (SearchContext is IWebElement element)
            builder.AppendLine().AppendLine().Append("Context element:").AppendLine().Append(element.ToDetailedString());
    }
}
