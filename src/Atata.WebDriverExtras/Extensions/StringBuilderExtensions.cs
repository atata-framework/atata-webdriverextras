namespace Atata;

/// <summary>
/// Provides a set of extension methods for <see cref="StringBuilder"/>.
/// </summary>
// TODO: v4. Remove StringBuilderExtensions.
public static class StringBuilderExtensions
{
    /// <summary>
    /// Appends the space character.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>A reference to the same <see cref="StringBuilder"/> instance after the append operation has completed.</returns>
    public static StringBuilder AppendSpace(this StringBuilder builder)
    {
        builder.CheckNotNull(nameof(builder));

        return builder.Append(' ');
    }
}
