namespace Atata;

/// <summary>
/// Provides a set of extension methods for <see cref="StringBuilder"/>.
/// </summary>
// TODO: v5. Remove StringBuilderExtensions.
public static class StringBuilderExtensions
{
    /// <summary>
    /// Appends the space character.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>A reference to the same <see cref="StringBuilder"/> instance after the append operation has completed.</returns>
    [Obsolete("Use Append(' ') instead.")] // Obsolete since v4.0.0.
    public static StringBuilder AppendSpace(this StringBuilder builder)
    {
        Guard.ThrowIfNull(builder);

        return builder.Append(' ');
    }
}
