namespace Atata
{
    /// <summary>
    /// Defines the thread boundary of <see cref="RetrySettings"/>.
    /// </summary>
    public enum RetrySettingsThreadBoundary
    {
        /// <summary>
        /// The <see cref="RetrySettings"/> values are thread-static (unique for each thread).
        /// </summary>
        ThreadStatic = 1,

        /// <summary>
        /// The <see cref="RetrySettings"/> values are static (same for all threads).
        /// </summary>
        Static,

        /// <summary>
        /// The <see cref="RetrySettings"/> values are unique for each given asynchronous control flow.
        /// </summary>
        AsyncLocal
    }
}
